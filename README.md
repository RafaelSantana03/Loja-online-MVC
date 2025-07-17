# RetroStore - Loja Online MVC

Este projeto foi desenvolvido com fins **educacionais**, como parte do estudo da stack **ASP.NET Core MVC**. A proposta é simular uma **loja online simples**, com produtos fictícios, carrinho de compras, e uma experiência completa de checkout com **pagamento via Pix** (QR Code).

## 🔧 Tecnologias Utilizadas

- ASP.NET Core MVC
- Entity Framework Core
- SQL Server LocalDB
- QRCoder (geração de QR Code Pix)
- Identity (autenticação de usuários)
- Bootstrap 5
- Session para carrinho de compras

## 🎯 Funcionalidades

- 🧾 Listagem de produtos com preços e imagens
- 🛒 Carrinho de compras com adição e remoção de produtos
- 💳 Pagamento via Pix com geração de QR Code
- 📦 Formulário de endereço para entrega antes do pagamento
- ✅ Confirmação do pedido após pagamento
- 👤 Autenticação de usuário com Identity
- 📄 Página de privacidade

### 🔒 Autenticação

O sistema utiliza o ASP.NET Identity para login e registro de usuários. É necessário estar autenticado para finalizar pedidos. Apenas usuários administradores têm acesso à opção "Gerenciar Produtos", onde podem cadastrar, editar e remover produtos do catálogo.

## 📸 Screenshots

### Catálogo de Produtos
<img width="1920" height="1080" alt="produtos" src="https://github.com/user-attachments/assets/ca76571f-9c0e-4217-bfce-e2691546704d" />

### Carrinho de Compras
<img width="1920" height="1080" alt="carrinhoComProduto" src="https://github.com/user-attachments/assets/0a2992cc-7850-4059-a414-ca3615e607cd" />

### Pagamento via Pix
<img width="1920" height="1080" alt="PagamentoPix" src="https://github.com/user-attachments/assets/debd0168-7d45-4dee-80c5-6196b5743326" />

### Confirmação de Pedido
<img width="1920" height="1080" alt="PedidoRelizado" src="https://github.com/user-attachments/assets/8f1820e7-776b-49ba-9142-47b4eadeba38" />

### Painel Administrativo (Apenas Admins)
<img width="1920" height="1080" alt="gerenciarProdutos" src="https://github.com/user-attachments/assets/19c201c5-baa8-4fa3-9587-5c39c2778681" />


## 🗂 Estrutura do Projeto

O projeto segue o padrão MVC do ASP.NET, com organização clara para controllers, models, views e área administrativa.

![image11](image11)

## 🚀 Como Executar

1. Clone o repositório:
    ```bash
    git clone https://github.com/RafaelSantana03/Loja-online-MVC.git
    ```
2. Configure a string de conexão do SQL Server LocalDB no arquivo `appsettings.json`.
3. Execute as migrações do Entity Framework Core:
    ```bash
    dotnet ef database update
    ```
4. Inicie a aplicação:
    ```bash
    dotnet run
    ```
5. Acesse `http://localhost:PORTA` no navegador.

## 📝 Licença

Este projeto está sob a licença MIT.

---
