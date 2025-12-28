# 🎬 Sistema de Gestão de Cinema

Sistema web completo para gestão de cinema desenvolvido em **ASP.NET Core MVC** com **Entity Framework Core** e **ASP.NET Identity**.

---

## 📋 Índice

- [Sobre o Projeto](#sobre-o-projeto)
- [Funcionalidades](#funcionalidades)
- [Tecnologias Utilizadas](#tecnologias-utilizadas)
- [Arquitetura](#arquitetura)
- [Pré-requisitos](#pré-requisitos)
- [Instalação e Configuração](#instalação-e-configuração)
- [Estrutura do Projeto](#estrutura-do-projeto)
- [Utilizadores Padrão](#utilizadores-padrão)
- [Funcionalidades por Role](#funcionalidades-por-role)
- [Modelos de Dados](#modelos-de-dados)
- [Capturas de Ecrã](#capturas-de-ecrã)
- [Melhorias Futuras](#melhorias-futuras)
- [Autor](#autor)

---

## 📖 Sobre o Projeto

O **Sistema de Gestão de Cinema** é uma aplicação web desenvolvida para o **Módulo 5 – TGPSI** que permite a gestão completa de um cinema, incluindo:

- Catálogo de filmes com upload de imagens
- Gestão de sessões de cinema
- Sistema de reservas de bilhetes
- Controlo de acessos diferenciado (Admin vs Cliente)

O sistema implementa boas práticas de desenvolvimento web, incluindo arquitetura MVC, separação de responsabilidades, validações robustas e interface moderna e responsiva.

---

## ✨ Funcionalidades

### 🎥 Gestão de Filmes (Admin)
- ✅ Criar, editar, visualizar e eliminar filmes
- ✅ Upload de imagens de capa
- ✅ Informações completas (título, género, duração, sinopse)
- ✅ Visualização de sessões associadas

### 📅 Gestão de Sessões (Admin)
- ✅ Criar, editar, visualizar e eliminar sessões
- ✅ Definir horário, sala e preço
- ✅ Filtros por filme e data
- ✅ Visualização de reservas por sessão

### 🎟️ Sistema de Reservas (Cliente)
- ✅ Visualizar catálogo de filmes e sessões
- ✅ Fazer reservas para sessões futuras
- ✅ Visualizar histórico de reservas
- ✅ Cancelar reservas (apenas para sessões futuras)

### 🔐 Autenticação e Autorização
- ✅ Registo de novos utilizadores
- ✅ Login/Logout
- ✅ Controlo de acessos por Roles (Admin/Cliente)
- ✅ Proteção de rotas sensíveis
- ✅ Página de acesso negado

---

## 🛠️ Tecnologias Utilizadas

### Backend
- **ASP.NET Core 8.0** - Framework MVC
- **Entity Framework Core** - ORM para acesso a dados
- **ASP.NET Identity** - Sistema de autenticação e autorização
- **SQLite** - Base de dados relacional

### Frontend
- **Razor Views** - Motor de templates
- **Bootstrap 5** - Framework CSS responsivo
- **Bootstrap Icons** - Ícones
- **jQuery** - Manipulação DOM e AJAX
- **jQuery Validation** - Validação client-side

### Padrões e Práticas
- **MVC (Model-View-Controller)** - Arquitetura
- **Repository Pattern** - Acesso a dados
- **ViewModel Pattern** - Separação de apresentação
- **Dependency Injection** - Inversão de controlo
- **Data Annotations** - Validações
- **Fluent API** - Configuração de relacionamentos

---

## 🏗️ Arquitetura

O projeto segue a arquitetura **MVC** com separação clara de responsabilidades:
```
┌─────────────────────────────────────────────┐
│              PRESENTATION LAYER             │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐   │
│  │  Views   │  │ View     │  │ Tag      │   │
│  │  (Razor) │  │ Models   │  │ Helpers  │   │
│  └──────────┘  └──────────┘  └──────────┘   │
└─────────────────────────────────────────────┘
                     ▼
┌─────────────────────────────────────────────┐
│             APPLICATION LAYER               │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐   │
│  │Controller│  │ ViewModels│  │ DTOs    │   │
│  └──────────┘  └──────────┘  └──────────┘   │
└─────────────────────────────────────────────┘
                     ▼
┌─────────────────────────────────────────────┐
│               BUSINESS LAYER                │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐   │
│  │ Models   │  │ Services │  │Validators│   │
│  └──────────┘  └──────────┘  └──────────┘   │
└─────────────────────────────────────────────┘
                     ▼
┌─────────────────────────────────────────────┐
│               DATA ACCESS LAYER             │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐   │
│  │DbContext │  │ Migrations│  │ Seed    │   │
│  └──────────┘  └──────────┘  └──────────┘   │
└─────────────────────────────────────────────┘
```

---

## 📦 Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- **.NET 8.0 SDK** ou superior
  - Download: https://dotnet.microsoft.com/download
- **Visual Studio Code** (recomendado) ou Visual Studio 2022
- **SQLite** (incluído no .NET)
- **Git** (opcional)

---

## 🚀 Instalação e Configuração

### 1️⃣ Clone o repositório
```bash
git clone https://github.com/seu-usuario/cinema-gabriel.git
cd cinema-gabriel
```

### 2️⃣ Restaurar dependências
```bash
dotnet restore
```

### 3️⃣ Configurar a base de dados

A connection string já está configurada em `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=cinema.db"
  }
}
```

### 4️⃣ Aplicar Migrations
```bash
dotnet ef database update
```

Isso criará o ficheiro `cinema.db` com todas as tabelas necessárias.

### 5️⃣ Criar pasta de uploads
```bash
mkdir wwwroot/uploads/filmes
```

### 6️⃣ Executar a aplicação
```bash
dotnet run
```

A aplicação estará disponível em: **http://localhost:5133**

---

## 📁 Estrutura do Projeto
```
CinemaGabriel/
├── Controllers/           # Controladores MVC
│   ├── AccountController.cs
│   ├── FilmesController.cs
│   ├── HomeController.cs
│   ├── ReservasController.cs
│   └── SessoesController.cs
├── Data/                  # Contexto e Seed
│   ├── ApplicationDbContext.cs
│   ├── PromoteToAdmin.cs
│   └── SeedData.cs
├── Migrations/            # Migrations do EF Core
├── Models/                # Modelos de domínio
│   ├── ApplicationUser.cs
│   ├── Filme.cs
│   ├── Reserva.cs
│   └── Sessao.cs
├── ViewModels/            # ViewModels
│   ├── AccountViewModels.cs
│   ├── FilmeViewModel.cs
│   ├── ReservaViewModel.cs
│   └── SessaoViewModel.cs
├── Views/                 # Views Razor
│   ├── Account/
│   ├── Filmes/
│   ├── Home/
│   ├── Reservas/
│   ├── Sessoes/
│   └── Shared/
│       ├── _Layout.cshtml
│       ├── _ValidationScriptsPartial.cshtml
│       ├── _ViewImports.cshtml
│       └── _ViewStart.cshtml
├── wwwroot/               # Ficheiros estáticos
│   ├── css/
│   ├── js/
│   ├── lib/
│   └── uploads/
│       └── filmes/        # Imagens dos filmes
├── appsettings.json       # Configurações
├── Program.cs             # Ponto de entrada
└── cinema.db              # Base de dados SQLite
```

---

## 👥 Utilizadores Padrão

O sistema cria automaticamente (via Seed) dois utilizadores de teste:

### 🔑 Administrador
- **Email:** `admin@cinema.pt`
- **Password:** `Admin@123`
- **Role:** Admin

### 🔑 Cliente
- **Email:** `cliente@cinema.pt`
- **Password:** `Cliente@123`
- **Role:** Cliente

⚠️ **Importante:** Alterar estas passwords em ambiente de produção!

---

## 🎭 Funcionalidades por Role

### 👨‍💼 Admin
- ✅ CRUD completo de Filmes
- ✅ CRUD completo de Sessões
- ✅ Visualizar TODAS as reservas do sistema
- ✅ Cancelar qualquer reserva (mesmo de clientes)
- ✅ Ver estatísticas de reservas por sessão

### 👤 Cliente
- ✅ Visualizar catálogo de filmes
- ✅ Visualizar sessões disponíveis
- ✅ Criar reservas
- ✅ Visualizar apenas suas próprias reservas
- ✅ Cancelar suas reservas (apenas para sessões futuras)

---

## 📊 Modelos de Dados

### Diagrama de Relacionamentos
```
┌─────────────────┐       ┌─────────────────┐
│     Filme       │       │  ApplicationUser│
├─────────────────┤       ├─────────────────┤
│ Id (PK)         │       │ Id (PK)         │
│ Titulo          │       │ NomeCompleto    │
│ Genero          │       │ Email           │
│ Duracao         │       │ DataRegistro    │
│ Sinopse         │       └─────────────────┘
│ CaminhoImagem   │                │
│ DataCadastro    │                │
└─────────────────┘                │
         │                         │
         │ 1:N                     │ 1:N
         ▼                         ▼
┌─────────────────┐       ┌─────────────────┐
│     Sessao      │       │     Reserva     │
├─────────────────┤       ├─────────────────┤
│ Id (PK)         │◄──────┤ Id (PK)         │
│ Horario         │  N:1  │ DataReserva     │
│ Sala            │       │ SessaoId (FK)   │
│ Preco           │       │ UserId (FK)     │
│ FilmeId (FK)    │       └─────────────────┘
└─────────────────┘
```

### Relacionamentos

- **Filme → Sessão**: 1:N (Um filme tem várias sessões)
- **Sessão → Reserva**: 1:N (Uma sessão tem várias reservas)
- **ApplicationUser → Reserva**: 1:N (Um utilizador tem várias reservas)

### Delete Behavior

Todos os relacionamentos usam **`CASCADE DELETE`**:
- Eliminar um Filme → elimina suas Sessões
- Eliminar uma Sessão → elimina suas Reservas
- Eliminar um Utilizador → elimina suas Reservas

---

## 🖼️ Capturas de Ecrã

### Página Inicial
![Home](docs/screenshots/home.png)

### Catálogo de Filmes
![Filmes](docs/screenshots/filmes.png)

### Detalhes do Filme
![Detalhes](docs/screenshots/filme-detalhes.png)

### Sessões Disponíveis
![Sessões](docs/screenshots/sessoes.png)

### Confirmação de Reserva
![Reserva](docs/screenshots/reserva-create.png)

### Minhas Reservas
![Minhas Reservas](docs/screenshots/reservas.png)

---

## 🔒 Segurança Implementada

### Autenticação
- ✅ ASP.NET Identity com hash de passwords
- ✅ Cookies de autenticação seguros
- ✅ Validação de email único

### Autorização
- ✅ Atributo `[Authorize]` em rotas protegidas
- ✅ Atributo `[Authorize(Roles = "Admin")]` para rotas de Admin
- ✅ Verificação de propriedade (Cliente só acede aos seus dados)

### Validações
- ✅ `[ValidateAntiForgeryToken]` contra ataques CSRF
- ✅ Data Annotations para validação de modelos
- ✅ Validações de negócio no controller
- ✅ Validação client-side com jQuery Validation

### Proteção de Dados
- ✅ Nomes únicos (GUID) para ficheiros enviados
- ✅ Validação de tipos de ficheiro (imagens)
- ✅ Proteção contra SQL Injection (via EF Core)
- ✅ Proteção contra XSS (Razor sanitiza HTML)

---

## 📈 Melhorias Futuras

### Funcionalidades
- [ ] Sistema de pagamento online
- [ ] Escolha de lugares na sala
- [ ] Notificações por email
- [ ] Avaliações e comentários de filmes
- [ ] Sistema de cupons/descontos
- [ ] Relatórios e estatísticas avançadas
- [ ] API REST para integração mobile

### Técnicas
- [ ] Implementar Unit Tests
- [ ] Adicionar cache (Redis)
- [ ] Migrar para SQL Server em produção
- [ ] Implementar CI/CD
- [ ] Adicionar logging estruturado (Serilog)
- [ ] Implementar soft delete
- [ ] Adicionar paginação em listagens

---

## 🧪 Testes

### Executar a aplicação
```bash
# Modo desenvolvimento
dotnet run

# Modo produção
dotnet run --configuration Release
```

### Testar funcionalidades

1. **Como Admin:**
   - Login: `admin@cinema.pt` / `Admin@123`
   - Criar filme com imagem
   - Criar sessão para o filme
   - Ver todas as reservas

2. **Como Cliente:**
   - Registar nova conta
   - Ver catálogo de filmes
   - Fazer reserva para uma sessão
   - Ver histórico de reservas
   - Cancelar reserva

---

## 📚 Boas Práticas Implementadas

### Código
- ✅ Nomenclatura clara e consistente
- ✅ Comentários XML em classes e métodos importantes
- ✅ Separação de responsabilidades (SRP)
- ✅ DRY (Don't Repeat Yourself)
- ✅ SOLID principles

### Arquitetura
- ✅ ViewModels para separar apresentação
- ✅ Async/Await em operações I/O
- ✅ Dependency Injection
- ✅ Repository Pattern via DbContext
- ✅ Fluent API para configurações complexas

### UI/UX
- ✅ Design responsivo (mobile-first)
- ✅ Feedback visual ao utilizador (TempData)
- ✅ Confirmações antes de ações destrutivas
- ✅ Ícones intuitivos
- ✅ Mensagens de erro claras

---

## 🐛 Resolução de Problemas

### Base de dados não criada
```bash
dotnet ef database drop --force
dotnet ef database update
```

### Erro "Role Admin does not exist"
```bash
# Apagar base de dados e recriar
del cinema.db
dotnet ef database update
dotnet run
```

### Imagens não aparecem
```bash
# Verificar se a pasta existe
mkdir wwwroot/uploads/filmes
```

### Admin não consegue ver reservas de clientes
```bash
# Verificar roles do admin
# Garantir que admin NÃO tem role Cliente
# Ver Program.cs - bloco de correção de roles
```

---

## 📝 Licença

Este projeto foi desenvolvido para fins académicos (Módulo 5 – TGPSI).

---

## 👨‍💻 Autor

**Gabriel**  
Curso: TGPSI  
Módulo: 5 - Programação Web Server-Side  
Ano Letivo: 2024/2025

---

## 🙏 Agradecimentos

- ASP.NET Core Documentation
- Entity Framework Core Documentation
- Bootstrap Documentation
- Stack Overflow Community

---

## 📞 Suporte

Para questões ou sugestões:
- 📧 Email: [pedro.henrique.lyra17@gmail.com]
---

**⭐ Se este projeto foi útil, considere dar uma estrela no GitHub!**
