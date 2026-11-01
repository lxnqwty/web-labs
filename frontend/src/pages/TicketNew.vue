<script setup>
import { nextTick, ref } from 'vue'
import { useRouter } from 'vue-router'
import { items, notice } from '../ticketsStore.js'
const router = useRouter()
const titleInput = ref(null)
const form = ref({ title: '', categoryId: '1', description: '' })
function createActivity() {
  const title = form.value.title.trim()
  const description = form.value.description.trim()
  if (title.length < 5 || title.length > 80 || description.length < 10 || description.length > 500) return
  const number = Math.max(1000, ...items.value.map(item => item.number)) + 1
  items.value.unshift({ id: Math.max(0, ...items.value.map(item => item.id)) + 1, number, title, description, categoryId: Number(form.value.categoryId), status: 'New' })
  form.value = { title: '', categoryId: '1', description: '' }
  notice.value = `Запись №${number} добавлена в общий список. Данные пока хранятся только в памяти браузера.`
  router.push('/')
}
nextTick(() => titleInput.value?.focus())
</script>
<template>
  <section>
    <div class="heading"><div><small>ЦИФРОВОЙ ПРОФИЛЬ</small><h1>Новая активность</h1><p>Добавьте информацию о своём достижении или результате.</p></div></div>
    <div class="form-layout">
      <section class="panel form-panel">
        <div class="form-title"><span>＋</span><div><h2>Информация об активности</h2><p>Заполните основные сведения о вашей активности.</p></div></div>
        <form @submit.prevent="createActivity">
          <div class="field"><label for="title">Тема активности <em>*</em></label><input id="title" ref="titleInput" v-model="form.title" name="title" minlength="5" maxlength="80" placeholder="Например, участие в конференции" required><small>От 5 до 80 символов</small></div>
          <div class="field"><label for="categoryId">Категория активности <em>*</em></label><select id="categoryId" v-model="form.categoryId" name="categoryId" required><option value="1">Учебная</option><option value="2">Научная</option><option value="3">Проектная</option></select><small>Выберите направление активности</small></div>
          <div class="field"><label for="description">Описание <em>*</em></label><textarea id="description" v-model="form.description" name="description" minlength="10" maxlength="500" rows="5" placeholder="Кратко опишите, что было сделано и какого результата удалось достичь..." required></textarea><small>От 10 до 500 символов · {{ form.description.length }}/500</small></div>
          <div class="form-actions"><router-link class="secondary action-link" to="/">Отмена</router-link><button class="primary" type="submit">Создать запись →</button></div>
        </form>
      </section>
      <aside class="aside"><span>✧</span><h3>Поделитесь своими успехами</h3><p>Добавляйте активности, чтобы постепенно формировать целостную картину ваших достижений.</p><hr><p><strong>Полезный совет</strong><br>Указывайте конкретные результаты и факты — так проверяющему будет проще оценить вашу работу.</p></aside>
    </div>
  </section>
</template>
