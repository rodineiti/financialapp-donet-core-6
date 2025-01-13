README - FinancialAppMvc

Descrição do Projeto

O FinancialAppMvc é uma aplicação web desenvolvida com ASP.NET Core 6 no padrão MVC. O objetivo do sistema é gerenciar o fluxo financeiro de usuários autenticados, permitindo o cadastro de entradas e saídas (transações financeiras), exibição de saldo em tempo real e auditoria do histórico. O projeto foi implementado utilizando as melhores práticas de design, incluindo Design Patterns, MediatR, e event-driven architecture.

Principais Funcionalidades 1. Autenticação e Autorização:
• Registro de usuários com envio de e-mail de ativação.
• Login e logout.
• Restrição de acesso às transações para usuários autenticados. 2. Gestão de Transações:
• Cadastro, edição, exclusão e listagem de transações financeiras.
• Cada transação está vinculada ao usuário autenticado, garantindo privacidade dos dados. 3. Saldo do Usuário (Balance):
• O saldo do usuário é armazenado em uma tabela separada (Balances) para garantir maior performance em cenários com grandes volumes de transações.
• As alterações no saldo são feitas automaticamente ao registrar ou excluir transações. 4. Auditoria (Audit Logs):
• Cada alteração no saldo do usuário é registrada em uma tabela de auditoria (AuditLogs) para rastreamento das operações.
• A auditoria inclui informações como o valor anterior e atual do saldo. 5. Arquitetura Orientada a Eventos (Event-Driven):
• As alterações no saldo e a auditoria são realizadas por meio de eventos usando MediatR.
• O envio de e-mails (ex: ativação de conta) também é gerenciado de forma assíncrona, evitando bloqueio no fluxo principal. 6. Monitoramento e Logging:
• Logs de transações e eventos são gerados para depuração e análise de desempenho.
• Logging configurado com possibilidade de uso de Serilog para armazenamento em arquivos ou bancos de dados.

Tecnologias Utilizadas
• Backend:
• ASP.NET Core 6 (MVC).
• Entity Framework Core (Code-First com SQLite para persistência de dados).
• MediatR (arquitetura orientada a eventos).
• Frontend:
• Razor Pages.
• Bootstrap (para estilização responsiva).
• Autenticação:
• Identity Framework com suporte para e-mail de confirmação.
• Logging e Monitoramento:
• Logging integrado com ILogger.
• Opção de configuração de Serilog.

Estrutura do Projeto

Camadas Principais 1. Models:
• Contém as classes representando as entidades do domínio, como Transaction, UserBalance, e AuditLog. 2. Controllers:
• Controladores responsáveis por gerenciar a interação entre a interface do usuário e os serviços, como TransactionsController. 3. Repositories:
• Implementações do padrão Repository para abstrair o acesso ao banco de dados. 4. Services:
• Contém a lógica de negócios e operações relacionadas a transações e usuários. 5. Events & Listeners:
• Gerenciamento de eventos, como alterações no saldo e auditorias.

Configurações e Setup

Pré-requisitos
• .NET 6 SDK instalado.
• SQLite (banco de dados local padrão).
• Ferramentas de IDE como Visual Studio ou Visual Studio Code.

Passos para Executar o Projeto 1. Clone o repositório:

git clone git@github.com:rodineiti/financialapp-donet-core-6.git
cd financialapp-donet-core-6

    2.	Restaure as dependências:

dotnet restore

    3.	Execute as migrações para criar o banco de dados:

dotnet ef database update

    4.	Compile e execute o projeto:

dotnet run

    5.	Acesse no navegador:

http://localhost:5000

Entidades

Transaction
• Id: Identificador único.
• Description: Descrição da transação.
• Amount: Valor da transação.
• Category: Categoria (Ex: Alimentação, Lazer).
• Type: Tipo (Entrada ou Saída).
• Date: Data da transação.
• UserId: Identificador do usuário que realizou a transação.

Balance
• Id: Identificador único.
• UserId: Identificador do usuário.
• CurrentBalance: Saldo atual do usuário.

AuditLog
• Id: Identificador único.
• UserId: Identificador do usuário.
• PreviousBalance: Saldo antes da operação.
• AmountChanged: Valor da transação.
• NewBalance: Saldo após a operação.
• Operation: Tipo de operação (Adição, Subtração).
• Timestamp: Data e hora da auditoria.

Principais Endpoints

Autenticação
• /Account/Register: Registro de novos usuários.
• /Account/Login: Login de usuários existentes.

Transações
• /Transactions: Lista todas as transações do usuário autenticado.
• /Transactions/Create: Página para criar uma nova transação.
• /Transactions/Edit/{id}: Página para editar uma transação existente.
• /Transactions/Delete/{id}: Exclui uma transação.

Demonstração de Lógica Chave

Alteração do Saldo com Event-Driven 1. Sempre que uma transação é criada ou excluída, um evento TransactionBalanceEvent é disparado. 2. O handler (TransactionBalanceEventHandler) processa o evento:
• Atualiza o saldo na tabela Balances.
• Registra a operação na tabela AuditLogs.

Código do Evento

public class TransactionBalanceEvent : INotification
{
public Transaction Transaction { get; set; }
public EventBalanceType EventBalanceType { get; set; }

    public TransactionBalanceEvent(Transaction transaction, EventBalanceType eventBalanceType)
    {
        Transaction = transaction;
        EventBalanceType = eventBalanceType;
    }

}

Código do Handler

public class TransactionBalanceEventHandler : INotificationHandler<TransactionBalanceEvent>
{
private readonly UserRepository \_userRepository;
private readonly AuditLogRepository \_auditLog;
private readonly BalanceStrategyFactory \_balanceStrategyFactory;

    public TransactionBalanceEventHandler(UserRepository userRepository, AuditLogRepository auditLog, BalanceStrategyFactory balanceStrategyFactory)
    {
        _userRepository = userRepository;
        _auditLog = auditLog;
        _balanceStrategyFactory = balanceStrategyFactory;
    }

    public async Task Handle(TransactionBalanceEvent notification, CancellationToken cancellationToken)
    {
        var balance = await _userRepository.GetBalanceUserAsync();

        if (balance == null)
        {
            balance = new Balance { UserId = notification.Transaction.UserId, CurrentBalance = 0 };
            await _userRepository.AddBalanceUserAsync(balance);
        }

        var previousBalance = balance.CurrentBalance;

        var balanceStrategy = _balanceStrategyFactory
            .CreateBalanceStrategy(notification.EventBalanceType, notification.Transaction.Type);

        await balanceStrategy.UpdateBalanceAsync(balance, notification.Transaction);

        await _auditLog.AddAuditLogAsync(new AuditLog
        {
            UserId = notification.Transaction.UserId,
            PreviousBalance = previousBalance,
            AmountChanged = notification.Transaction.Amount,
            NewBalance = balance.CurrentBalance
        });
    }

}

Melhorias Futuras
• Implementar filtros avançados para as transações (por categoria, data, etc.).
• Suporte para gráficos e visualizações financeiras.
• Integração com APIs externas (ex: para câmbio ou pagamentos).
• Suporte para múltiplas moedas.

Contribuição

Sinta-se à vontade para enviar issues ou pull requests. Qualquer feedback é bem-vindo!

Licença

Este projeto está licenciado sob a MIT License.
