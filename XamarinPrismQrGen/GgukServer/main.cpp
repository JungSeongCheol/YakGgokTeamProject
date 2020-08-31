#define _CRT_SECURE_NO_WARNINGS
#include <windows.h>
#include <Winsock.h>
#include <iostream>
#include <vector>
#include <sstream>
#include <mutex>

#include "mysql_connection.h"
#include "Client.h"

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

const sql::SQLString server = "tcp://127.0.0.1:3306";
const sql::SQLString username = "root";
const sql::SQLString password = "mysql_p@ssw0rd";

std::vector<Client*> connections;
std::mutex mClient;

std::vector<std::string> getTokens(std::string input, char delimiter) {
	std::vector<std::string> tokens;
	std::istringstream f(input);
	std::string s;
	while (std::getline(f, s, delimiter)) {
		tokens.push_back(s);
	}
	return tokens;
}

void ServerThread(Client* client) {
	char* sent = new char[256];
	char* received = new char[256];
	int size = 0;

	sql::Connection* con;
	sql::Driver* driver;
	sql::PreparedStatement* pstmt;
	sql::Statement* stmt;
	sql::ResultSet* result;

	client->setMutex(&mClient);

	//로그인
	while (true) {
		ZeroMemory(received, 256);
		if ((size = recv(client->getClientSocket(), received, 256, NULL)) > 0) {
			std::string receivedString = std::string(received);
			std::vector<std::string> tokens = getTokens(receivedString, ']');
			try {
				driver = get_driver_instance();
				con = driver->connect(server, username, password);
			}
			catch (sql::SQLException e) {
				std::cout << e.what() << std::endl;
				return;
			}

			con->setSchema("member");

			if (receivedString.find("[Login]") != -1) {
				std::vector<std::string> dataTokens = getTokens(tokens[1], ',');
				std::string id = dataTokens[0];
				std::string password = dataTokens[1];

				pstmt = con->prepareStatement("SELECT * FROM pmemtbl WHERE Id=? AND Pwd=?");
				pstmt->setString(1, id.c_str());
				pstmt->setString(2, password.c_str());
				result = pstmt->executeQuery();

				if (result->next()) {

					bool loginChk = client->setClientID(id, connections);
					if (loginChk == false) {
						send(client->getClientSocket(), "AlreadyLogin", 4, 0);
					}
					else {
						mClient.lock();
						connections.push_back(client);
						mClient.unlock();

						send(client->getClientSocket(), "LogIn", 5, 0);
						break;
					}
				}
				else {
					send(client->getClientSocket(), "Fail", 4, 0);
				}
			}
			else if (receivedString.find("[SignUp]") != -1) {
				std::vector<std::string> dataTokens = getTokens(tokens[1], ',');
				std::string tbl = dataTokens[0];
				std::string id = dataTokens[1];
				std::string password = dataTokens[2];
				std::string name = dataTokens[3];
				std::string phone = dataTokens[4];

				if (tbl == "pmemtbl") {
					pstmt = con->prepareStatement("INSERT INTO pmemtbl(Id, Pwd, Name, Phone) VALUES(?,?,?,?)");

					pstmt->setString(1, id.c_str());
					pstmt->setString(2, password.c_str());
					pstmt->setString(3, name.c_str());
					pstmt->setString(4, phone.c_str());
				}
				else {
					pstmt = con->prepareStatement("");
				}

				try {
					int r = pstmt->executeUpdate();
					send(client->getClientSocket(), "succ", 4, 0);
				}
				catch (sql::SQLException e) {
					send(client->getClientSocket(), "fail", 4, 0);
				}
			}
		}
		else {
			if (client->getClientID() == "") {

				closesocket(client->getClientSocket());
				return;
			}
		}
	}

	std::cout << "[ 새로운 사용자 접속 ] " << client->getClientID() << std::endl;
	while (true) {
		if ((size = recv(client->getClientSocket(), received, 256, NULL)) > 0) {

		}
		else {
			ZeroMemory(sent, 256);
			sprintf(sent, "클라이언트 [%s]의 연결이 끊어졌습니다.", client->getClientID().c_str());
			std::cout << sent << std::endl;
			/* 게임에서 나간 플레이어를 찾기 */
			mClient.lock();
			for (int i = 0; i < connections.size(); i++) {
				if (connections[i]->getClientID() == client->getClientID()) {
					connections.erase(connections.begin() + i);
					break;
					/* 다른 사용자와 게임 중이던 사람이 나간 경우 */
					//if (connections[i]->getRoomID() != -1 &&
					//	clientCountInRoom(connections[i]->getRoomID()) == 2) {
					//	/* 남아있는 사람에게 메시지 전송 */
					//	exitClient(connections[i]->getRoomID());
					//}
					//connections.erase(connections.begin() + i);

					//std::string msg = "[People]";
					//for (int i = 0; i < connections.size(); i++) {
					//	msg.append(connections[i]->getClientID());
					//	msg.append(",");
					//}
					////conn.erase(conn.end());

					//for (int i = 0; i < connections.size(); i++) {
					//	send(connections[i]->getClientSocket(), msg.c_str(), msg.length() - 1, 0);
					//}
					//break;
				}
			}
			mClient.unlock();
			delete client;
			con->close();
			closesocket(client->getClientSocket());
			return;
		}
	}

}

int main() {
	WSAStartup(MAKEWORD(2, 2), &wsaData);
	serverSocket = socket(AF_INET, SOCK_STREAM, NULL);

	serverAddress.sin_addr.s_addr = htonl(INADDR_ANY);
	serverAddress.sin_port = htons(9876);
	serverAddress.sin_family = AF_INET;

	std::cout << "[GGuk 서버 가동 ]" << std::endl;
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



#pragma region 임시 파일
//
//class Client;
//
//std::mutex mClient;
//std::vector<Client*> connections;
//
//class Client {
//private:
//	std::string clientID;
//
//	SOCKET clientSocket;
//public:
//	Client(SOCKET clientSocket) {
//		this->clientSocket = clientSocket;
//	} // 클라이언트 생성시 초기값.
//
//	std::string getClientID() {
//		return clientID;
//	}
//
//	bool setClientID(std::string client, std::vector<Client*> connections) {
//		mClient.lock();
//		for (int i = 0; i < connections.size(); i++) {
//			if (client == connections[i]->getClientID()) {
//				mClient.unlock();	// 중복ID제거 (쿼리시 중복있으면 false 반환을 하기위해)
//				return false;
//			}
//		}
//		mClient.unlock();
//		this->clientID = client;
//		return true;
//	}
//
//	SOCKET getClientSocket() {
//		return clientSocket;
//	}
//};
//
//SOCKET serverSocket;
//WSAData wsaData;
//SOCKADDR_IN serverAddress;
//
//std::vector<std::string> getTokens(std::string input, char delimiter) {
//	std::vector<std::string> tokens;
//	std::istringstream f(input);
//	std::string s;
//	while (std::getline(f, s, delimiter)) {
//		tokens.push_back(s);
//	}
//	return tokens;
//}
//
//const sql::SQLString server = "tcp://192.168.0.165:3306";
//const sql::SQLString username = "root";
//const sql::SQLString password = "mysql_p@ssw0rd";
//
//void ServerThread(Client* client) {
//	char* sent = new char[256];
//	char* received = new char[256];
//	int size = 0;
//
//	while (true) {
//		ZeroMemory(received, 256);
//		if ((size = recv(client->getClientSocket(), received, 256, NULL)) > 0) {
//			std::string receivedString = std::string(received); // 같은 문자로 만듬
//			std::vector<std::string> tokens = getTokens(receivedString, ']');
//			sql::Driver* driver;
//			sql::Connection* con;
//			sql::PreparedStatement* pstmt;
//			sql::Statement* stmt;
//			sql::ResultSet* result;
//
//			try
//			{
//				driver = get_driver_instance();
//				con = driver->connect(server, username, password);
//			}
//			catch (std::exception)
//			{
//				std::cout << "SQL error" << std::endl;
//				return;
//			}
//
//			con->setSchema("member");
//
//			if (receivedString.find("[Login]") != -1) {
//				std::vector<std::string> dataTokens = getTokens(tokens[1], ',');
//				std::string id = dataTokens[0];
//				std::string password = dataTokens[1];
//
//				pstmt = con->prepareStatement("SELECT * FROM pmemtbl WHERE Id=? AND Pwd=?");
//				pstmt->setString(1, id.c_str());
//				pstmt->setString(2, password.c_str());
//				result = pstmt->executeQuery();
//
//				if (result->next()) {
//					bool login = client->setClientID(id, connections);
//					if (login == false) {
//						send(client->getClientSocket(), "already", 7, NULL);
//					}
//					else {
//						mClient.lock();
//						connections.push_back(client);
//						mClient.unlock();
//
//						send(client->getClientSocket(), "OK", 2, NULL);
//						break;
//					}
//				}
//				else {
//					send(client->getClientSocket(), "invalid", 7, NULL);
//				}
//			}
//
//			else if (receivedString.find("[SignUp]") != -1) {
//				std::vector<std::string> dataTokens = getTokens(tokens[1], ',');
//				std::string table = dataTokens[1];
//				std::string id = dataTokens[2];
//				std::string password = dataTokens[3];
//				std::string Name = dataTokens[4];
//				std::string Phone = dataTokens[5];
//
//				if (table == "pmemtbl") {
//					pstmt = con->prepareStatement("INSERT INTO pmemtbl(Id, Pwd, Name, Phone) VALUES (? ,? ,? ,?)");
//					pstmt->setString(1, id.c_str());
//					pstmt->setString(2, password.c_str());
//					pstmt->setString(3, id.c_str());
//					pstmt->setString(4, password.c_str());
//
//					try {
//						int r = pstmt->executeUpdate();
//						send(client->getClientSocket(), "SignUpOk", 8, NULL);
//					}
//					catch (sql::SQLException e) {
//						send(client->getClientSocket(), "SignUpFalse", 11, NULL);
//					}
//				}
//			}
//		}
//		else {
//			closesocket(client->getClient)
//			break;
//		}
//	}
//
//	std::cout << "[ 새로운 사용자 [" << client->getClientID() << "] 접속 ]" << std::endl;
//
//	while (true) {
//		ZeroMemory(received, 256);
//		if ((size = recv(client->getClientSocket(), received, 256, NULL)) > 0) {
//			;
//		}
//		else {
//			sprintf(sent, "클라이언트 [%s]의 연결이 끊어졌습니다.", client->getClientID().c_str());
//			delete client;
//			break;
//		}
//	}
//}
//int main() {
//
//
//	WSAStartup(MAKEWORD(2, 2), &wsaData); // MAKEWORD 0x0202가 반환됨, WSAStratup - 소켓함수의 시작.
//	serverSocket = socket(AF_INET, SOCK_STREAM, NULL);
//
//	serverAddress.sin_addr.s_addr = htonl(INADDR_ANY);
//	serverAddress.sin_port = htons(9876);
//	serverAddress.sin_family = AF_INET;
//
//	std::cout << "[Gguk 서버 가동]" << std::endl;
//	bind(serverSocket, (SOCKADDR*)&serverAddress, sizeof(serverAddress));
//	listen(serverSocket, 32);
//
//	int addressLength = sizeof(serverAddress);
//	while (true) {
//		SOCKET clientSocket = socket(AF_INET, SOCK_STREAM, NULL);
//		if (clientSocket = accept(serverSocket, (SOCKADDR*)&serverAddress, &addressLength)) {
//			Client* client = new Client(clientSocket);
//			CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)ServerThread, (LPVOID)client, NULL, NULL);
//		}
//		Sleep(100);
//	}
//}
#pragma endregion

