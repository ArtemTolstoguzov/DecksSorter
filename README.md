# DecksSorter
RESTful сервис, реализованный на ASP.NET Core с использованием Docker Compose, PostgreSQL и EF Core и предоставляющий интерфейс:
* Создать именованную колоду карт (колода создаётся упорядоченной)
* Удалить именованную колоду
* Получить список названий колод
* Перетасовать колоду
* Получить колоду по имени (в её текущем упорядоченном/перетасованном состоянии)

## Запуск
1. `git clone https://github.com/ArtemTolstoguzov/DecksSorter`
2. `cd DecksSorter`
3. `docker-compose build`
4. `docker-compose up`
5.  Перейти на http://localhost:8000/swagger

### Алгоритм перетасовки
Алгоритм перетасовки задается в DI-контейнере в файле `Startup.cs`:
`services.AddScoped<IShuffler, SimpleShuffler>();` (по умолчанию стоит "простая")
