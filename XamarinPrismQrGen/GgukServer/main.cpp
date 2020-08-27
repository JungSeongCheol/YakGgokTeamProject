#include <windows.h>
#include <Winsock.h>
#include <iostream>
#include <vector>
#include <sstream>
#include <mutex>

#include "mysql_connection.h"

#include <cppconn/driver.h>
#include <cppconn/exception.h>
#include <cppconn/resultset.h>
#include <cppconn/statement.h>
#include <cppconn/prepared_statement.h>

#pragma comment (lib, "ws2_32.lib")

#pragma comment(lib, "mysqlcppconn.lib")


SOCKET serverSocket;
WSAData wsaData;
SOCKADDR_IN serverAddress;

int main() {


	WSAStartup(MAKEWORD(2, 2), &wsaData); // MAKEWORD 0x0202�� ��ȯ��, WSAStratup - �����Լ��� ����.
	serverSocket = socket(AF_INET, SOCK_STREAM, NULL);

	serverAddress.sin_addr.s_addr = htonl(INADDR_ANY);
	serverAddress.sin_port = htons(9876);
	serverAddress.sin_family = AF_INET;

	std::cout << "[ C++ ���� ���� ���� ���� ]" << std::endl;
	bind(serverSocket, (SOCKADDR*)&serverAddress, sizeof(serverAddress));
	listen(serverSocket, 32);

	int addressLength = sizeof(serverAddress);
	while (true) {
		SOCKET clientSocket = socket(AF_INET, SOCK_STREAM, NULL);
		if (clientSocket = accept(serverSocket, (SOCKADDR*)&serverAddress, &addressLength)) {
			Client* client = new Client(clientSocket);
			CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)ServerThread, (LPVOID)client, NULL, NULL);
		}
		Sleep(100);
	}
}