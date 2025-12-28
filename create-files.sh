#!/bin/bash

# Models
touch Models/Filme.cs
touch Models/Sessao.cs
touch Models/Reserva.cs
touch Models/ApplicationUser.cs

# ViewModels
touch ViewModels/AccountViewModels.cs

# Data
touch Data/ApplicationDbContext.cs
touch Data/SeedData.cs

# Controllers
touch Controllers/AccountController.cs
touch Controllers/FilmesController.cs
touch Controllers/SessoesController.cs
touch Controllers/ReservasController.cs
touch Controllers/UtilizadoresController.cs

# Views - Account
touch Views/Account/Login.cshtml
touch Views/Account/Register.cshtml
touch Views/Account/AccessDenied.cshtml

# Views - Filmes
touch Views/Filmes/Index.cshtml
touch Views/Filmes/Create.cshtml
touch Views/Filmes/Edit.cshtml
touch Views/Filmes/Details.cshtml
touch Views/Filmes/Delete.cshtml

# Views - Sessoes
touch Views/Sessoes/Index.cshtml
touch Views/Sessoes/Create.cshtml
touch Views/Sessoes/Edit.cshtml
touch Views/Sessoes/Details.cshtml
touch Views/Sessoes/Delete.cshtml

# Views - Reservas
touch Views/Reservas/Index.cshtml
touch Views/Reservas/Create.cshtml
touch Views/Reservas/Details.cshtml
touch Views/Reservas/Delete.cshtml

# Views - Utilizadores
touch Views/Utilizadores/Index.cshtml
touch Views/Utilizadores/Details.cshtml
touch Views/Utilizadores/Delete.cshtml

# Views - Shared
touch Views/Shared/_ValidationScriptsPartial.cshtml

echo "✅ Todos os ficheiros criados!"
