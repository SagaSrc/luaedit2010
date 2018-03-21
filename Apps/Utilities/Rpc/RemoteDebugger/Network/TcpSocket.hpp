#ifndef NETWORK_TCPSOCKET_HPP
#define NETWORK_TCPSOCKET_HPP

#include <iostream>
#include <sstream>
#include <string>
#include <winsock2.h>

#include <boost/noncopyable.hpp>

namespace network
{
	class TcpSocket : private boost::noncopyable
	{
	public:
		TcpSocket();
		~TcpSocket();

		// Connection related functions
		void Listen(int aiPort);
		int Accept();
		inline bool IsConnected() { return mIsConnected; }
		void Close();

		// Data related functions
		void ReceiveBuffer(char* aBuffer, int aiLength);
		std::string ReceiveString(int aiLength);
		int ReceiveInt();

		void SendBuffer(const char* aBuffer, int aiLength);
		void SendString(std::string aStr);
		void SendInt(int aiInt);

	private:
		SOCKET mConnectionSocket; // Socket used for connection
		SOCKET mSocket; // Actual socket
		bool mIsConnected;
	};
}

#endif //NETWORK_TCPSOCKET_HPP
