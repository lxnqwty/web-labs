import { mockCreateTicket, mockGetTickets } from './mockTickets.js'

// Для перехода на настоящий backend достаточно переключить значение на false.
const USE_MOCK = true
const baseUrl = import.meta.env.VITE_API_BASE_URL || 'http://localhost:5000'

export class ApiError extends Error {
  constructor(message, status = null) {
    super(message)
    this.name = 'ApiError'
    this.status = status
  }
}

async function readResponse(response) {
  let data = {}

  try {
    data = await response.json()
  } catch {
    data = {}
  }

  if (!response.ok) {
    throw new ApiError(data.message || `Ошибка HTTP: ${response.status}`, response.status)
  }

  return data
}

async function request(path, options = {}) {
  let response

  try {
    response = await fetch(`${baseUrl}${path}`, {
      ...options,
      headers: {
        ...(options.body ? { 'Content-Type': 'application/json' } : {}),
        ...options.headers,
      },
    })
  } catch (error) {
    // Не маскируем сетевой сбой под HTTP 500: у него нет HTTP-статуса.
    throw new ApiError('Нет ответа от сервера.', null)
  }

  return readResponse(response)
}

async function resolveMock(response) {
  if (!response.ok) {
    throw new ApiError(response.data?.message || 'Запрос отклонён.', response.status)
  }

  return response.data
}

export async function getTickets() {
  if (USE_MOCK) {
    return resolveMock(await mockGetTickets())
  }

  const data = await request('/api/tickets')
  return Array.isArray(data) ? data : data.items
}

export async function createTicket(body) {
  if (USE_MOCK) {
    return resolveMock(await mockCreateTicket(body))
  }

  return request('/api/tickets', {
    method: 'POST',
    body: JSON.stringify(body),
  })
}
