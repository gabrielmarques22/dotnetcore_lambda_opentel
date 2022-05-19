
# Implementação OpenTel em Lambda .NetCore + Dynatrace

Tutorial da Lambda: https://awswith.net/2020/02/11/the-simplest-dot-net-core-aws-lambda-function/

Adicionar os pacotes:
  dotnet add package **OpenTelemetry.Contrib.Instrumentation.AWS**
  dotnet add package **OpenTelemetry.Exporter.OpenTelemetryProtocol**
  
Pacote lambda para dotnet (facilita a geração do pacote): 

 - (Instalação) **dotnet tool install --global Amazon.Lambda.Tools --version 5.4.1**
 - (Geração do Pacote(zip)) **dotnet lambda package SuaFunção.zip**  

Definir variáveis de Ambiente na lambda:
|Chave|Valor|
|--|--|
| OTEL_EXPORTER_OTLP_ENDPOINT | https://(TENANT_ID).live.dynatrace.com/api/v2/otlp |
| OTEL_EXPORTER_OTLP_HEADERS| Authorization=Api-Token (SEU TOKEN) |
| OTEL_EXPORTER_OTLP_PROTOCOL|   http/protobuf |

## Conceitos Importantes

 - **ActivitySource e Activity**: são as implementações do OpenTelemetry no .NET para TracerProvider (objeto responsável por enviar o trace) e Span (o trace em si). As classes fazem parte do pacote (System.Diagnostics) e não do OpenTelemetry. A API do OpenTel, simplesmente usa esses objetos
 - **Variáveis de Ambiente**: Mesmo sendo possível setar o endpoint do exporter por código como mostra na documentação, acho que fica mais organizado e fácil de implementar por variável de ambiente. Basta adicionar o método **.AddOtlpExporter()** na criação do **Sdk.CreateTracerProviderBuilder()** e ele busca nas variáveis de ambiente da lambda
 - **Nome do Serviço**: o nome do serviço que vai aparecer no Dyna pode ser adicionado com o método .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("NOME DO SERVIÇO")) na criação do TracerProvider. Atualmente o Dyna gera uma página para concentrar as informações dos traces desse "serviço"
