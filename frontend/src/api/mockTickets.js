const delay = (ms = 500) => new Promise((resolve) => setTimeout(resolve, ms))

let nextId = 4

// Управляемые сценарии для проверки состояний без настоящего сервера.
export const mockSettings = {
  emptyList: false,
  forceListError: false,
}

const initialTickets = [
  {
    id: 1,
    number: 1001,
    title: 'Участие в научной конференции',
    description: 'Подготовлен и представлен доклад по результатам учебного исследования.',
    categoryId: 2,
    status: 'Closed',
  },
  {
    id: 2,
    number: 1002,
    title: 'Разработка командного веб-проекта',
    description: 'Создание прототипа цифрового сервиса в рамках проектной деятельности.',
    categoryId: 3,
    status: 'InProgress',
  },
  {
    id: 3,
    number: 1003,
    title: 'Успешное завершение учебного модуля',
    description: 'Выполнены практические задания по профильной дисциплине.',
    categoryId: 1,
    status: 'New',
  },
]

let tickets = initialTickets.map((ticket) => ({ ...ticket }))

export async function mockGetTickets() {
  await delay()

  if (mockSettings.forceListError) {
    return {
      ok: false,
      status: 500,
      data: { message: 'Учебная ошибка загрузки списка.' },
    }
  }

  return {
    ok: true,
    status: 200,
    data: mockSettings.emptyList ? [] : tickets.map((ticket) => ({ ...ticket })),
  }
}

export async function mockCreateTicket(body) {
  await delay()

  if (body.force500) {
    return {
      ok: false,
      status: 500,
      data: { message: 'Сервер не смог обработать запрос.' },
    }
  }

  if (body.force400 || !body.title || body.title.trim().length < 5) {
    return {
      ok: false,
      status: 400,
      data: { message: 'Тема должна содержать не менее 5 символов.' },
    }
  }

  const created = {
    id: nextId++,
    number: 1000 + nextId - 1,
    title: body.title.trim(),
    categoryId: Number(body.categoryId),
    description: body.description.trim(),
    status: 'New',
  }

  tickets.unshift(created)

  return {
    ok: true,
    status: 201,
    data: { ...created },
  }
}
