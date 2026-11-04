<script setup>
import { nextTick, ref } from 'vue'
import { useRouter } from 'vue-router'
import { createTicket } from '../api/tickets.js'
import { addItem, categories, notice } from '../ticketsStore.js'

const router = useRouter()
const titleInput = ref(null)
const form = ref({ title: '', categoryId: 1, description: '' })
const errors = ref({ title: '', categoryId: '', description: '' })
const isSubmitting = ref(false)
const force400 = ref(false)
const force500 = ref(false)
const submitError = ref('')

async function onCreate() {
  if (isSubmitting.value) return

  const title = form.value.title.trim()
  const description = form.value.description.trim()
  const categoryId = form.value.categoryId

  errors.value = { title: '', categoryId: '', description: '' }
  submitError.value = ''

  if (title.length < 5 || title.length > 80) {
    errors.value.title = 'Тема должна содержать от 5 до 80 символов.'
  }

  if (!Number.isInteger(categoryId) || categoryId <= 0 || !categories.some((category) => category.id === categoryId)) {
    errors.value.categoryId = 'Выберите существующую категорию.'
  }

  if (description.length < 10 || description.length > 500) {
    errors.value.description = 'Описание должно содержать от 10 до 500 символов.'
  }

  if (errors.value.title || errors.value.categoryId || errors.value.description) return

  isSubmitting.value = true

  try {
    const created = await createTicket({
      title,
      categoryId,
      description,
      force400: force400.value,
      force500: force500.value,
    })

    addItem(created)
    form.value = { title: '', categoryId: 1, description: '' }
    force400.value = false
    force500.value = false
    notice.value = `Запись #${created.number} добавлена в общий список. Идентификатор получен из ответа API.`
    router.push('/')
  } catch (error) {
    if (error.status === 400) {
      submitError.value = 'Проверьте поля: сервер отклонил введённые данные.'
    } else if (error.status === 500) {
      submitError.value = 'Сервер не смог обработать запрос. Попробуйте позже.'
    } else {
      submitError.value = 'Нет связи с сервером. Проверьте подключение и повторите попытку.'
    }
  } finally {
    isSubmitting.value = false
  }
}

nextTick(() => titleInput.value?.focus())
</script>

<template>
  <section>
    <div class="heading"><div><small>ЦИФРОВОЙ ПРОФИЛЬ</small><h1>Новая активность</h1><p>Добавьте информацию о своём достижении или результате.</p></div></div>
    <div class="form-layout">
      <section class="panel form-panel">
        <div class="form-title"><span>＋</span><div><h2>Информация об активности</h2><p>Заполните основные сведения о вашей активности.</p></div></div>
        <form @submit.prevent="onCreate" novalidate>
          <div class="field">
            <label for="title">Тема активности <em>*</em></label>
            <input id="title" ref="titleInput" v-model="form.title" name="title" minlength="5" maxlength="80" placeholder="Например, участие в конференции" required :aria-invalid="Boolean(errors.title)" aria-describedby="title-help title-error">
            <small id="title-help">От 5 до 80 символов</small>
            <small v-if="errors.title" id="title-error" class="field-error" role="alert">{{ errors.title }}</small>
          </div>
          <div class="field">
            <label for="categoryId">Категория активности <em>*</em></label>
            <select id="categoryId" v-model.number="form.categoryId" name="categoryId" required :aria-invalid="Boolean(errors.categoryId)" aria-describedby="category-error">
              <option v-for="category in categories" :key="category.id" :value="category.id">{{ category.name }}</option>
            </select>
            <small>Выберите направление активности</small>
            <small v-if="errors.categoryId" id="category-error" class="field-error" role="alert">{{ errors.categoryId }}</small>
          </div>
          <div class="field">
            <label for="description">Описание <em>*</em></label>
            <textarea id="description" v-model="form.description" name="description" minlength="10" maxlength="500" rows="5" placeholder="Кратко опишите, что было сделано и какого результата удалось достичь..." required :aria-invalid="Boolean(errors.description)" aria-describedby="description-help description-error"></textarea>
            <small id="description-help">От 10 до 500 символов · {{ form.description.length }}/500</small>
            <small v-if="errors.description" id="description-error" class="field-error" role="alert">{{ errors.description }}</small>
          </div>
          <label class="mock-switch"><input v-model="force400" type="checkbox"> Имитировать ответ сервера HTTP 400 (учебный сценарий)</label>
          <label class="mock-switch"><input v-model="force500" type="checkbox"> Имитировать ответ сервера HTTP 500 (учебный сценарий)</label>
          <p v-if="submitError" class="api-form-error" role="alert">{{ submitError }}</p>
          <div class="form-actions">
            <router-link class="secondary action-link" to="/">Отмена</router-link>
            <button class="primary" type="submit" :disabled="isSubmitting">{{ isSubmitting ? 'Создаём…' : 'Создать запись →' }}</button>
          </div>
        </form>
      </section>
      <aside class="aside"><span>✧</span><h3>Поделитесь своими успехами</h3><p>Добавляйте активности, чтобы постепенно формировать целостную картину ваших достижений.</p><hr><p><strong>Полезный совет</strong><br>Указывайте конкретные результаты и факты — так проверяющему будет проще оценить вашу работу.</p></aside>
    </div>
  </section>
</template>
