# ExpenseControlSystem - ASP.NET Core
Projeto de controle de gastos no qual o usuário pode cadastrar pessoas, categorias, subcategorias e despesas. Cada despesa é vinculada a um usuário e a uma subcategoria, enquanto cada subcategoria está associada a uma categoria.

Projeto desenvolvido para demonstrar conhecimentos em **APIs REST** com **ASP.NET Core**.

---

## Tecnologias

- ASP.NET Core
- Entity Framework Core
- SQLite
- LINQ
- Swagger

---

## Funcionalidades

- CRUD completo de usuários
- CRUD completo de categorias e subcategorias
- CRUD completo de despesas
- Paginação de resultados
- Filtros por categoria, subcategoria e usuário
- Filtro por despesas pagas e não pagas (`IsPaid`)
- Validação de dados com DataAnnotations
- Padronização de respostas da API
- Envio de Email com relatório de gastos de um usuario e/ou empresa com filtros opcionais de digida paga, não paga e atrasada.

---

## Padrão de Resposta

```json
{
  "data": {},
  "errors": []
}
```

---

## Principais Rotas da API

### Buscar despesas
`GET /v1/expenses`

**Query params:**
- `page` (obrigatório)
- `pageSize` (obrigatório)
- `userId` (opcional)
- `subCategoryId` (opcional)
- `isPaid` (opcional)
  - `true` → despesas pagas
  - `false` → despesas não pagas
- `LatePayment` (opcional)
  - `true` → despesas não pagas em atraso
  - `false` → despesas não pagas dentro do prazo de pagamento

---

## Banco de Dados

- SQLite
- Criado via Entity Framework Core Migrations
- Arquivo do banco não versionado no repositório

---

## Como Executar

```bash
git clone https://github.com/gabrielfl1/ExpenseControlSystem.git
cd ExpenseControlSystem
dotnet ef database update
dotnet run
```

Acesse o Swagger em:
```
https://localhost:{porta}/swagger
```
Ou se preferir baixe a collection do postman neste link:  
[postman collection.json](https://github.com/gabrielfl1/ExpenseControlSystem/blob/main/Postman/Expense%20Control%20System.postman_collection.json)

---

## Autor

Gabriel Ferreira Lima  
Projeto de portfólio para demonstração de conhecimento em APIs com .NET
