Sistema web desenvolvido em C# com ASP.NET Core MVC para gerenciamento de equipamentos de TI em ambiente corporativo.

## Objetivo

O projeto foi criado com base em um problema real: a ausência de controle eficiente sobre equipamentos em manutenção dentro do setor de infraestrutura.

A aplicação permite:

* Cadastro de equipamentos
* Controle de status (em manutenção, concluído ou condenado)
* Registro de responsável pela manutenção
* Histórico de movimentações
* Consulta por patrimônio
* Organização por setor/localização

## Tecnologias utilizadas

* C#
* ASP.NET Core MVC
* Entity Framework Core
* MySQL

## Como executar o projeto

1. Clone o repositório
2. Configure a string de conexão com o banco de dados no arquivo `appsettings.json`
3. Execute as migrations (se aplicável)
4. Rode o projeto no Visual Studio (F5)

## Estrutura do projeto

* `Program.cs`
  Configuração inicial da aplicação, serviços e conexão com o banco

* `Controllers/`

  * `HomeController.cs` → páginas iniciais
  * `EquipamentosController.cs` → lógica de gerenciamento de equipamentos

* `Models/`

  * `Equipamento.cs` → entidade principal do sistema

* `Views/`
  Interfaces do usuário (não incluídas aqui)

## Funcionalidades principais

### Equipamentos

* Listagem de equipamentos em aberto
* Cadastro com validação de patrimônio único
* Edição controlada (sem sobrescrever dados críticos)
* Exclusão com confirmação

### Status

* Conclusão de manutenção com registro de data/hora
* Condenação de equipamento com motivo e reposição

### Histórico

* Visualização de equipamentos concluídos
* Visualização de equipamentos condenados

## Regras de negócio implementadas

* Um patrimônio não pode ser duplicado
* Equipamentos concluídos e condenados não aparecem na lista principal
* Registro de data e hora em ações críticas (conclusão e condenação)

## Status do projeto

Em desenvolvimento (~50% concluído)

Funcionalidades futuras (planejadas):

* Sistema de tarefas
* Guias de instalação mais avançada (Guia de instalação de programas por setores)
* Histórico detalhado por equipamento (log de alterações)
* Controle de usuários (quem fez cada ação)
* Integração com outros setores envolvidos como por exemplo: Auditoria, Manutenção, etc.

## Observações

Este é um projeto de estudo com foco em prática de desenvolvimento backend e resolução de problemas reais do ambiente de trabalho.
