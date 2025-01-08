Mensageria Project - Sistema de Envio de E-mails com Azure Service Bus

Este é um projeto que integra o Azure Service Bus com o serviço de envio de e-mails usando a API Resend. O sistema consome mensagens de uma fila do Azure Service Bus e, a partir dessas mensagens, envia e-mails aos destinatários especificados.

Tecnologias Utilizadas

-ASP.NET Core: Framework para construção da aplicação web.

-Azure Service Bus: Serviço de mensageria da Microsoft utilizado para enviar e receber mensagens de forma assíncrona.

-API Resend: API externa para o envio de e-mails.

-HttpClient: Utilizado para fazer as requisições HTTP à API Resend.

-Dependency Injection: Injeção de dependência para organizar o código e facilitar a manutenção.
Funcionalidade Principal

O sistema executa as seguintes funções:

-Envio de Mensagens: Envia dados para uma fila no Azure Service Bus.

-Recepção de Mensagens: Consome as mensagens dessa fila, processa os dados e envia e-mails para os destinatários utilizando a API Resend.

