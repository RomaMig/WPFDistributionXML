Решение состоит из 4 проектов:
1. Client;
2. Server;
3. TcpIO;
4. XmlParser.

Client - проект, описывающий логику поведения клиента;
Server - проект, описывающий логику поведения сервера;
TcpIO - проект, описывающий логику передачи сообщений по протоколу TCP;
XmlParser - проект, описывающий логику парсинга XML-файла.

Для полной сборки решения потребуется собрать проекты Client и Server.
TcpIO и XmlParser являются зависимостями Client и Server.
