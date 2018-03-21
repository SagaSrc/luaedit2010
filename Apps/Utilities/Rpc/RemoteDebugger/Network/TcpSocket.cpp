#include "TcpSocket.hpp"

#include <boost/scoped_ptr.hpp>

namespace network
{
	/////////////////////////////////////////////////////////////////////
	TcpSocket::TcpSocket() :
		mConnectionSocket(0),
		mSocket(0),
		mIsConnected(false)
	{
		mConnectionSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

		if (mConnectionSocket == INVALID_SOCKET)
		{
			std::cerr << std::endl << "Invalid socket! Last error: " << WSAGetLastError() << std::endl;
		}
	}

	/////////////////////////////////////////////////////////////////////
	TcpSocket::~TcpSocket()
	{
		Close();
	}

	/////////////////////////////////////////////////////////////////////
	void TcpSocket::Close()
	{
		if (mConnectionSocket != INVALID_SOCKET)
		{
			shutdown(mConnectionSocket, SD_BOTH);
			closesocket(mConnectionSocket);
			mConnectionSocket = INVALID_SOCKET;
		}

		if (mSocket != INVALID_SOCKET)
		{
			shutdown(mSocket, SD_BOTH);
			closesocket(mSocket);
			mSocket = INVALID_SOCKET;
		}
	}

	/////////////////////////////////////////////////////////////////////
	void TcpSocket::Listen(int aiPort)
	{
		sockaddr_in sa;

		memset(&sa, 0, sizeof(sa));
		sa.sin_family = PF_INET;
		sa.sin_port = htons(aiPort);

		// Bind the socket to the internet address
		if (bind(mConnectionSocket, (sockaddr *)&sa, sizeof(sockaddr_in)) == SOCKET_ERROR)
		{
            int errCode = WSAGetLastError();
			closesocket(mConnectionSocket);
            std::cerr << std::endl << "Cannot bind socket! Last error: " << errCode << "." << std::endl;
		}
		  
		listen(mConnectionSocket, 1);
	}

	/////////////////////////////////////////////////////////////////////
	int TcpSocket::Accept()
	{
		mSocket = accept(mConnectionSocket, 0, 0);

		if (mSocket == INVALID_SOCKET)
		{
			int errcode = WSAGetLastError();

			if(errcode == WSAEWOULDBLOCK)
			{
				return 0; // non-blocking call, no request pending
			}
			else
			{
				return errcode;
			}
		}
		else
		{
			// Set the socket to non-blocking mode
			u_long nNoBlock = 1;
            if (ioctlsocket(mSocket, FIONBIO, &nNoBlock) != SOCKET_ERROR)
			{
				mIsConnected = true;
			}
		}

		return 0;
	}

	/////////////////////////////////////////////////////////////////////
	void TcpSocket::ReceiveBuffer(char* aBuffer, int aiLength)
	{
		if (mIsConnected)
		{
			int totalReceived = 0;

			while (totalReceived < aiLength)
			{
				int rv = recv(mSocket, &aBuffer[totalReceived], aiLength, 0);
				totalReceived += rv;
				
				if (rv <= 0)
				{
					int err = WSAGetLastError();
					if (err != WSAEWOULDBLOCK)
					{
						// Failed to receive data... disconnected!
						mIsConnected = false;
					}

					break;
				}
			}
		}
	}

	/////////////////////////////////////////////////////////////////////
	std::string TcpSocket::ReceiveString(int aiLength)
	{
		std::string ret = "";

		if (mIsConnected)
		{
			int totalReceived = 0;
			boost::scoped_ptr<char> buf(new char[aiLength + 1]);
		 
			while (totalReceived < aiLength)
			{
				int rv = recv(mSocket, buf.get(), aiLength, 0);
				totalReceived += rv;
				
				if (rv > 0)
				{
					buf.get()[rv] = '\0';
					ret += buf.get();
				}
				else
				{
					int err = WSAGetLastError();
					if (err != WSAEWOULDBLOCK)
					{
						// Failed to receive data... disconnected!
						mIsConnected = false;
					}

					break;
				}
			}
		}

		return ret;
	}

	int TcpSocket::ReceiveInt()
	{
		int res = 0;

		if (mIsConnected)
		{
			char buf[sizeof(int)];
			int rv = recv(mSocket, buf, sizeof(int), 0);

			if (rv <= 0)
			{
				int err = WSAGetLastError();
				if (err != WSAEWOULDBLOCK)
				{
					// Failed to receive data... disconnected!
					mIsConnected = false;
				}

				return res;
			}

			memcpy(&res, buf, sizeof(buf));
		}

		return res;
	}

	/////////////////////////////////////////////////////////////////////
	void TcpSocket::SendBuffer(const char* aBuffer, int aiLength)
	{
		if (mIsConnected)
		{
			send(mSocket, aBuffer, aiLength, 0);
		}
	}

	/////////////////////////////////////////////////////////////////////
	void TcpSocket::SendString(std::string aStr)
	{
		if (mIsConnected)
		{
			send(mSocket, aStr.c_str(), (int)aStr.length() + 1, 0);
		}
	}

	/////////////////////////////////////////////////////////////////////
	void TcpSocket::SendInt(int aiInt)
	{
		if (mIsConnected)
		{
			char buf[sizeof(aiInt)];
			memcpy(buf, &aiInt, sizeof(buf));
			send(mSocket, buf, sizeof(buf), 0);
		}
	}
}