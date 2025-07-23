# 📘 Gerador de Tiagões

Sistema para gerenciamento de questões, matérias, disciplinas e geração automatizada de testes.

---

## 🎯 Objetivo

Organizar e facilitar a criação de provas e gabaritos por meio de um sistema web que permita:

- Cadastro e gerenciamento de Disciplinas, Matérias e Questões.
- Geração automática de testes com base em critérios definidos.
- Exportação de provas e gabaritos em PDF.
- Aplicação de regras de negócio específicas.

---

## 🚀 Tecnologias e Arquitetura

- ASP.NET Core MVC (.NET 8)
- Entity Framework Core
- SQL Server
- HTML + Bootstrap
- AutoMapper
- Arquitetura em 3 Camadas (Domain, Application, Infrastructure, WebApp)

---

## 📂 Estrutura do Projeto

GeradorDeTiagoes/
│
├── Domain/ → Entidades e Regras de Negócio
├── Application/ → Serviços de aplicação e validações
├── Structure.Orm/ → Persistência com EF Core
└──  WebApp/ → Interface MVC (Views, Controllers, ViewModels)
└── Tests/ → Testes unitários (Não implmentado AINDA)

## 📌 Funcionalidades

### ✅ Disciplinas
- Cadastro, edição, exclusão e listagem
- Nome obrigatório e único
- Bloqueio de exclusão se houver matérias ou testes associados

### ✅ Matérias
- Vinculadas a uma disciplina
- Nome e série obrigatórios
- Nome único dentro da disciplina
- Bloqueio de exclusão se houver questões associadas

### ✅ Questões
- Cadastro com enunciado, alternativas (2 a 4)
- Apenas uma alternativa correta
- Bloqueio de exclusão se a questão estiver em testes

### ✅ Testes
- Geração automática de questões por disciplina, série e recuperação
- Visualização e duplicação de testes
- Exportação de teste e gabarito em PDF
- Regras como: sem duplicar questões, selecionar matérias da disciplina, etc.

---

## 📅 Etapas do Projeto

1. **Planejamento**
   - Especificação de requisitos
   - Organização por módulos
   - Roadmap e issues no GitHub

2. **Desenvolvimento**
   - Entidades, repositórios e serviços
   - Views e controllers
   - Regras de negócio validadas por camada de serviço

3. **Validação e Testes**
   - Validação de formulários com DataAnnotations

4. **Extras**
   - Exportação em PDF
   - UI responsiva
   - Publicação futura em nuvem (Azure)

---

## 🧪 Testes

- Testes unitários nas entidades e serviços
- Casos validados:
  - Exclusão bloqueada
  - Alternativas inválidas
  - Geração correta de teste com questões variadas
- **Cypress** e **Lighthouse** não foram implementados ainda.
