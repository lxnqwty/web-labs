import { mockCreateTicket, mockGetTickets } from './mockTickets.js'

// Для локальной демонстрации mock можно включить через VITE_USE_MOCK=true.
const USE_MOCK = import.meta.env.VITE_USE_MOCK === 'true'
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
    throw new ApiError(
      data.detail || data.title || data.message || `Ошибка HTTP: ${response.status}`,
      response.status,
    )
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
  } catch {
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

export async function getTicket(id) {
  return request(`/api/tickets/${id}`)
}

export async function createTicket(body) {
  if (USE_MOCK) {
    return resolveMock(await mockCreateTicket(body))
  }

  const { force400, force500, ...payload } = body
  return request('/api/tickets', {
    method: 'POST',
    body: JSON.stringify(payload),
  })
}

export async function assignTicket(id, assigneeUserId) {
  return request(`/api/tickets/${id}/assignee`, {
    method: 'PATCH',
    body: JSON.stringify({ assigneeUserId }),
  })
}

export async function changeTicketStatus(id, status, currentUserId) {
  return request(`/api/tickets/${id}/status`, {
    method: 'PATCH',
    headers: currentUserId ? { 'X-User-Id': String(currentUserId) } : {},
    body: JSON.stringify({ status }),
  })
}
