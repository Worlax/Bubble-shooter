Небольшое тестовое задание которое я сделал за пару дней. Шарики рисовал в figma, логика поиска соседних шариков - bfs. Ничего сложного, старался делать аккуратные коммиты и работал относительно выделенного времени. При других временных рамках я бы написал другую логику поиска соседних шариков (на данный момент это физический поиск, т.е. объекты ищутся в непосредственной близости друг от друга), думаю я бы сделал отдельный класс "сетки" который отвечал бы за расположение шариков на сцене, заполнение пустых пространств и тригеры "падания" шариков, если они перестают "крепиться" к потолку.

https://user-images.githubusercontent.com/22938118/201130952-9be1dd15-ca58-4018-8322-20a34c76ec07.mp4

Текст задания:

● Создать прототип игры на Unity и предоставить результат в
соответствии с требованиями;
● Обязательно использовать следующую версию Unity 2021.3.0f.

Требования к игре:
Игра - Bubble shooter. Примеры:
● Bubble Shooter Genies - Apps on Google Play
● Bubble Shooter - Apps on Google Play
В прототипе должны быть предоставлены следующие механики:

● Стрельба шариком определенного цвета в заданную игроком
сторону;
● Разрушение шариков одинакового цвета при попадании;
● Отскакивание шарика от стенок и потолка;
● Если шарик не попал ни в один шарик своего цвета, он должен
“прилипнуть” к остальным шарикам.
Должна быть поддержка управления с компьютера (мышью) и с
телефона.
Уровень может быть двух типов:

● Заранее созданный;
● Случайно сгенерированный (алгоритм генерации можно выбрать
максимально простой).
Последовательность цветов шаров также может быть заданной
(цепочка цветов, которая бесконечно повторяется) или случайной.
В игре должен быть предоставлен минимальный интерфейс, который
включает себя:

● Экран главного меню;
● Внутриигровой интерфейс;
● Меню паузы.
Графическое оформление остаётся на ваше усмотрение.
Игра должна выглядеть завершённой, должны быть предоставлены
базовые возможности, в том числе перезапуск уровня, навигация
между экранами, выбор уровня.
