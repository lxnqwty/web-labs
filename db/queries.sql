-- ЛР9: обязательные запросы к учебным данным.

-- 1. Фильтр: только новые активности.
SELECT id, number, title, status, created_at
FROM activities
WHERE status = 'New';

-- 2. Сортировка: последние созданные записи сверху.
SELECT number, title, status, created_at
FROM activities
ORDER BY created_at DESC, id DESC;

-- 3. JOIN: показать название категории и автора, а не только внешние ключи.
SELECT
    a.number,
    a.title,
    c.name AS category_name,
    creator.full_name AS created_by,
    assignee.full_name AS reviewer,
    a.status
FROM activities AS a
JOIN categories AS c ON c.id = a.category_id
JOIN users AS creator ON creator.id = a.created_by_user_id
LEFT JOIN users AS assignee ON assignee.id = a.assignee_user_id
ORDER BY a.created_at DESC, a.id DESC;

-- 4. GROUP BY: количество активностей в каждом из четырёх статусов.
SELECT status, COUNT(*) AS activity_count
FROM activities
GROUP BY status
ORDER BY status;
