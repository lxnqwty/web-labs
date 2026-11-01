<script setup>
import { computed, nextTick, ref } from 'vue'

const screen = ref('list')
const filter = ref('Все')
const search = ref('')
const notice = ref('')
const titleInput = ref(null)

const categories = [
  { id: 1, name: 'Учебная' },
  { id: 2, name: 'Научная' },
  { id: 3, name: 'Проектная' },
]
const statusLabels = {
  New: 'Новая',
  InProgress: 'На проверке',
  Closed: 'Подтверждена',
  Cancelled: 'Отклонена',
}
const filters = ['Все', 'New', 'InProgress', 'Closed', 'Cancelled']
const activities = ref([
  { id: 1, number: 1001, title: 'Участие в научной конференции', description: 'Подготовлен и представлен доклад по результатам учебного исследования.', categoryId: 2, status: 'Closed' },
  { id: 2, number: 1002, title: 'Разработка командного веб-проекта', description: 'Создание прототипа цифрового сервиса в рамках проектной деятельности.', categoryId: 3, status: 'InProgress' },
  { id: 3, number: 1003, title: 'Успешное завершение учебного модуля', description: 'Выполнены практические задания по профильной дисциплине.', categoryId: 1, status: 'New' },
])
const form = ref({ title: '', categoryId: '1', description: '' })
const login = ref({ username: '', password: '' })

const visibleActivities = computed(() => {
  const query = search.value.trim().toLocaleLowerCase('ru')
  return activities.value.filter((item) => {
    const statusMatch = filter.value === 'Все' || item.status === filter.value
    const queryMatch = !query || `${item.number} ${item.title}`.toLocaleLowerCase('ru').includes(query)
    return statusMatch && queryMatch
  })
})
const stats = computed(() => ({
  total: activities.value.length,
  review: activities.value.filter((item) => item.status === 'InProgress').length,
  approved: activities.value.filter((item) => item.status === 'Closed').length,
}))
function categoryName(id) {
  return categories.find((item) => item.id === Number(id))?.name ?? `Категория ${id}`
}
function openScreen(value) {
  screen.value = value
  notice.value = ''
  if (value === 'new') nextTick(() => titleInput.value?.focus())
}
function createActivity() {
  const title = form.value.title.trim()
  const description = form.value.description.trim()
  if (title.length < 5 || title.length > 80 || description.length < 10 || description.length > 500) return
  const number = Math.max(1000, ...activities.value.map((item) => item.number)) + 1
  activities.value.unshift({
    id: Math.max(0, ...activities.value.map((item) => item.id)) + 1,
    number, title, description,
    categoryId: Number(form.value.categoryId),
    status: 'New',
  })
  form.value = { title: '', categoryId: '1', description: '' }
  filter.value = 'Все'
  search.value = ''
  notice.value = `Запись №${number} добавлена в демонстрационный список. Данные пока хранятся только в памяти браузера.`
  screen.value = 'list'
}
function submitLogin() {
  notice.value = 'Экран входа готов. Проверка учётных данных будет подключена вместе с API в следующей лабораторной работе.'
}
</script>

<template>
  <div class="app-shell">
    <aside class="sidebar">
      <a class="brand" href="#" @click.prevent="openScreen('list')">
        <span class="brand-mark">П</span>
        <span><strong>Профиль</strong><small>цифровая среда студента</small></span>
      </a>
      <p class="nav-caption">РАБОЧЕЕ ПРОСТРАНСТВО</p>
      <nav aria-label="Основная навигация">
        <button :class="{ active: screen === 'list' }" type="button" @click="openScreen('list')"><span>▤</span> Мои активности <b>{{ stats.total }}</b></button>
        <button :class="{ active: screen === 'new' }" type="button" @click="openScreen('new')"><span>＋</span> Добавить запись</button>
      </nav>
      <div class="sidebar-bottom">
        <div class="tip"><span>✦</span><strong>Ваши достижения</strong><p>Собирайте учебные, научные и проектные результаты в одном месте.</p></div>
        <button class="user-button" type="button" @click="openScreen('login')"><span class="avatar">АК</span><span><strong>Андрей Карабаев</strong><small>Студент</small></span><i>•••</i></button>
      </div>
    </aside>

    <main class="main-content">
      <header class="topbar">
        <div class="crumb">Рабочее пространство <span>/</span> <strong>{{ screen === 'list' ? 'Мои активности' : screen === 'new' ? 'Новая запись' : 'Вход в систему' }}</strong></div>
        <div class="mode"><i></i> Демонстрационный режим <span class="avatar">АК</span></div>
      </header>

      <div class="page">
        <div v-if="notice" class="notice" role="status"><span>✓</span><p>{{ notice }}</p><button type="button" aria-label="Закрыть" @click="notice = ''">×</button></div>

        <section v-if="screen === 'list'">
          <div class="heading">
            <div><small>ВАШ ЦИФРОВОЙ ПРОФИЛЬ</small><h1>Мои активности</h1><p>Отслеживайте свои достижения и результаты за время обучения.</p></div>
            <button class="primary" type="button" @click="openScreen('new')">＋ Добавить активность</button>
          </div>
          <div class="stats">
            <article><span>Всего записей</span><strong>{{ stats.total }}</strong><small>В вашем профиле</small></article>
            <article><span>На проверке</span><strong>{{ stats.review }}</strong><small>Ожидают подтверждения</small></article>
            <article><span>Подтверждено</span><strong>{{ stats.approved }}</strong><small>Успешные достижения</small></article>
          </div>
          <section class="panel" aria-labelledby="activity-title">
            <div class="panel-head"><div><h2 id="activity-title">История активностей</h2><p>Все ваши учебные и внеучебные результаты</p></div><label class="search">⌕ <input v-model="search" type="search" placeholder="Поиск по номеру или теме" aria-label="Поиск по номеру или теме"></label></div>
            <div class="filters"><button v-for="item in filters" :key="item" type="button" :class="{ selected: filter === item }" @click="filter = item">{{ item === 'Все' ? 'Все записи' : statusLabels[item] }}</button></div>
            <div class="table-wrap"><table>
              <thead><tr><th>НОМЕР</th><th>ТЕМА АКТИВНОСТИ</th><th>КАТЕГОРИЯ</th><th>СТАТУС</th></tr></thead>
              <tbody>
                <tr v-for="item in visibleActivities" :key="item.id">
                  <td class="number">#{{ item.number }}</td>
                  <td><strong class="activity-name">{{ item.title }}</strong><small class="subline">Запись в цифровом профиле</small></td>
                  <td><span class="category">{{ categoryName(item.categoryId) }}</span></td>
                  <td><span :class="['status', 'status-' + item.status.toLowerCase()]"><i></i>{{ statusLabels[item.status] }}</span></td>
                </tr>
                <tr v-if="visibleActivities.length === 0"><td colspan="4" class="empty">Ничего не найдено. Измените запрос или фильтр.</td></tr>
              </tbody>
            </table></div>
            <div class="panel-foot">Показано записей: <strong>{{ visibleActivities.length }}</strong><span>Демонстрационные данные</span></div>
          </section>
          <p class="footnote">ⓘ Сейчас отображаются тестовые записи. Сохранение на сервер будет добавлено на следующем этапе.</p>
        </section>

        <section v-else-if="screen === 'new'">
          <div class="heading"><div><small>ЦИФРОВОЙ ПРОФИЛЬ</small><h1>Новая активность</h1><p>Добавьте информацию о своём достижении или результате.</p></div></div>
          <div class="form-layout">
            <section class="panel form-panel">
              <div class="form-title"><span>＋</span><div><h2>Информация об активности</h2><p>Заполните основные сведения о вашей активности.</p></div></div>
              <form @submit.prevent="createActivity">
                <div class="field"><label for="title">Тема активности <em>*</em></label><input id="title" ref="titleInput" v-model="form.title" name="title" minlength="5" maxlength="80" placeholder="Например, участие в конференции" required><small>От 5 до 80 символов</small></div>
                <div class="field"><label for="categoryId">Категория активности <em>*</em></label><select id="categoryId" v-model="form.categoryId" name="categoryId" required><option value="1">Учебная</option><option value="2">Научная</option><option value="3">Проектная</option></select><small>Выберите направление активности</small></div>
                <div class="field"><label for="description">Описание <em>*</em></label><textarea id="description" v-model="form.description" name="description" minlength="10" maxlength="500" rows="5" placeholder="Кратко опишите, что было сделано и какого результата удалось достичь..." required></textarea><small>От 10 до 500 символов · {{ form.description.length }}/500</small></div>
                <div class="form-actions"><button class="secondary" type="button" @click="openScreen('list')">Отмена</button><button class="primary" type="submit">Создать запись →</button></div>
              </form>
            </section>
            <aside class="aside"><span>✧</span><h3>Поделитесь своими успехами</h3><p>Добавляйте активности, чтобы постепенно формировать целостную картину ваших достижений.</p><hr><p><strong>Полезный совет</strong><br>Указывайте конкретные результаты и факты — так проверяющему будет проще оценить вашу работу.</p></aside>
          </div>
        </section>

        <section v-else class="login-area">
          <div class="login-card"><div class="brand-mark">П</div><small>С РАДОСТЬЮ ВИДИМ ВАС СНОВА</small><h1>Вход в профиль</h1><p>Войдите, чтобы управлять своими активностями.</p>
            <form @submit.prevent="submitLogin">
              <div class="field"><label for="login">Логин</label><input id="login" v-model="login.username" name="login" autocomplete="username" placeholder="Введите логин" required></div>
              <div class="field"><label for="password">Пароль</label><input id="password" v-model="login.password" name="password" type="password" autocomplete="current-password" placeholder="Введите пароль" required></div>
              <button class="primary" type="submit">Войти в систему →</button>
            </form><small class="login-note">Демонстрационный экран · авторизация пока не подключена</small>
          </div>
        </section>
      </div>
      <footer>© 2026 Цифровой профиль студента <span>Учебный проект · Лабораторная работа №3</span></footer>
    </main>
  </div>
</template>

<style>
@import url('https://fonts.googleapis.com/css2?family=DM+Sans:wght@400;500;600;700&family=Manrope:wght@500;600;700;800&display=swap');
:root{font-family:'DM Sans',Arial,sans-serif;color:#202b3c;background:#f5f7fb;font-synthesis:none;text-rendering:optimizeLegibility}*{box-sizing:border-box}body{margin:0;min-width:320px}button,input,textarea,select{font:inherit}button{cursor:pointer}.app-shell{min-height:100vh}.sidebar{position:fixed;inset:0 auto 0 0;width:250px;background:white;border-right:1px solid #e9edf3;padding:27px 18px;display:flex;flex-direction:column}.brand{display:flex;align-items:center;gap:11px;color:inherit;text-decoration:none;padding:0 8px}.brand-mark{width:39px;height:39px;border-radius:12px;background:#315be9;color:white;display:grid;place-items:center;font:800 21px Manrope;box-shadow:0 6px 15px #315be933}.brand strong,.brand small,.user-button strong,.user-button small{display:block}.brand strong{font:800 16px Manrope}.brand small{font-size:10px;color:#929cad;margin-top:2px}.nav-caption{font-size:9px;letter-spacing:1px;color:#a0a9b8;font-weight:700;margin:48px 10px 13px}.sidebar nav{display:grid;gap:5px}.sidebar nav button{height:43px;border:0;border-radius:9px;background:transparent;color:#68758a;text-align:left;padding:0 12px;font-size:12px;font-weight:600}.sidebar nav button span{font-size:17px;margin-right:11px}.sidebar nav button b{float:right;border-radius:20px;background:#dce6ff;color:#315be9;padding:3px 7px;font-size:10px}.sidebar nav button.active{background:#edf2ff;color:#315be9}.sidebar-bottom{margin-top:auto}.tip{background:#f4f6fc;border:1px solid #edf0f7;border-radius:12px;padding:14px;margin-bottom:17px}.tip>span{display:block;color:#4265e8;margin-bottom:8px}.tip strong{font:700 12px Manrope}.tip p{font-size:10px;line-height:1.6;color:#8792a4;margin:6px 0 0}.user-button{width:100%;display:flex;align-items:center;gap:9px;padding:13px 4px 0;border:0;border-top:1px solid #edf0f4;background:white;text-align:left}.avatar{width:31px;height:31px;border-radius:50%;background:#e8edff;color:#365be2;display:grid;place-items:center;font-size:10px;font-weight:700}.user-button strong{font-size:10px}.user-button small{font-size:9px;color:#929cad;margin-top:3px}.user-button i{margin-left:auto;color:#9aa4b4;font-style:normal}.main-content{margin-left:250px;min-height:100vh;display:flex;flex-direction:column}.topbar{height:65px;background:white;border-bottom:1px solid #e9edf3;display:flex;align-items:center;justify-content:space-between;padding:0 42px}.crumb{font-size:11px;color:#9aa4b4}.crumb span{padding:0 9px;color:#c4cbd5}.crumb strong{color:#465267}.mode{display:flex;align-items:center;gap:8px;color:#7f8b9e;font-size:10px}.mode i{width:6px;height:6px;background:#28b77b;border-radius:50%;box-shadow:0 0 0 3px #e3f7ee}.page{width:min(1120px,100%);margin:0 auto;padding:42px 42px 28px;flex:1}.heading{display:flex;align-items:flex-end;justify-content:space-between;gap:20px;margin-bottom:27px}.heading small,.login-card>small{font-size:9px;letter-spacing:1.1px;color:#6880d9;font-weight:700}.heading h1,.login-card h1{font:800 27px Manrope;margin:8px 0 0;letter-spacing:-.8px}.heading p,.panel-head p{font-size:12px;color:#8a95a6;margin:7px 0 0}.primary{height:40px;border:0;border-radius:8px;background:#315be9;color:white;padding:0 16px;font-size:11px;font-weight:700;box-shadow:0 5px 12px #315be925}.primary:hover{background:#244bd2}.stats{display:grid;grid-template-columns:repeat(3,1fr);gap:14px;margin-bottom:22px}.stats article,.panel{background:white;border:1px solid #edf0f5;border-radius:11px;box-shadow:0 2px 7px #1d293906}.stats article{padding:16px 18px;min-height:105px}.stats article span,.stats article small{display:block;font-size:10px;color:#8792a3}.stats article strong{display:block;font:800 23px Manrope;margin:7px 0 1px}.stats article small{font-size:9px;color:#a1aaba}.panel-head{display:flex;align-items:center;justify-content:space-between;gap:18px;padding:20px 20px 14px}.panel-head h2,.form-title h2{font:700 14px Manrope;margin:0}.panel-head p,.form-title p{font-size:10px;color:#9aa4b3;margin:5px 0 0}.search{height:34px;width:225px;border:1px solid #e8ecf2;border-radius:7px;display:flex;align-items:center;padding:0 10px;gap:7px;color:#a4adba;font-size:19px}.search input{width:100%;border:0;outline:0;font-size:10px}.filters{display:flex;gap:7px;padding:0 20px 13px}.filters button{border:1px solid #e9edf3;background:white;border-radius:20px;padding:6px 10px;color:#7e899a;font-size:9px}.filters button.selected{border-color:#dce5ff;background:#edf2ff;color:#315be9;font-weight:700}.table-wrap{overflow-x:auto}table{width:100%;border-collapse:collapse;text-align:left}thead{background:#f8f9fc}th{height:37px;padding:0 14px;color:#a0a9b7;font-size:8px;letter-spacing:.6px}td{height:64px;border-top:1px solid #f0f2f6;padding:8px 14px;font-size:10px;color:#667286;white-space:nowrap}th:first-child,td:first-child{padding-left:20px}.number{font:700 10px Manrope;color:#65738b}.activity-name{display:block;color:#354156;font-size:10px}.subline{display:block;font-size:9px;color:#a3adba;margin-top:4px}.category{display:inline-block;padding:5px 9px;border-radius:5px;background:#f3f5fa;color:#69768c;font-size:9px}.status{display:inline-flex;align-items:center;gap:6px;padding:5px 9px;border-radius:20px;font-size:9px;font-weight:600}.status i{width:5px;height:5px;border-radius:50%;background:currentColor}.status-new{background:#eef2ff;color:#536fe0}.status-inprogress{background:#fff5e6;color:#c58a25}.status-closed{background:#e9f8f0;color:#239765}.status-cancelled{background:#fff0f0;color:#d06464}.empty{text-align:center;color:#929cad;height:100px}.panel-foot{display:flex;justify-content:space-between;padding:12px 20px;border-top:1px solid #f0f2f6;color:#a0a9b7;font-size:9px}.panel-foot strong{color:#68758a}.footnote{font-size:9px;color:#9aa4b3;margin:14px 2px}.form-layout{display:grid;grid-template-columns:minmax(0,1fr) 245px;gap:18px;align-items:start}.form-panel{padding:22px 24px}.form-title{display:flex;gap:12px;align-items:center;padding-bottom:18px;border-bottom:1px solid #eff1f5}.form-title>span{width:34px;height:34px;display:grid;place-items:center;border-radius:9px;background:#edf2ff;color:#315be9;font-size:19px}.form-panel form,.login-card form{padding-top:20px}.field{display:grid;gap:8px;margin-bottom:17px}.field label{font-size:10px;font-weight:700;color:#48556a}.field em{color:#e36d6d;font-style:normal}.field input,.field textarea,.field select{width:100%;border:1px solid #e4e9f0;border-radius:7px;background:white;padding:10px 12px;color:#344156;font-size:11px;outline:none}.field input,.field select{height:39px}.field textarea{resize:vertical;line-height:1.5}.field input:focus,.field textarea:focus,.field select:focus{border-color:#8299f0;box-shadow:0 0 0 3px #315be912}.field input::placeholder,.field textarea::placeholder{color:#b2bac6}.field>small{font-size:9px;color:#a2acba;margin-top:-3px}.form-actions{display:flex;justify-content:flex-end;gap:9px;padding-top:12px;border-top:1px solid #eff1f5}.secondary{height:39px;padding:0 16px;border:1px solid #e4e9f0;border-radius:7px;background:white;color:#718096;font-size:10px}.aside{background:#eef2ff;border:1px solid #e5eaff;border-radius:11px;padding:20px 18px;color:#5267ad}.aside>span{display:grid;place-items:center;width:42px;height:42px;border-radius:12px;background:#dfe7ff;font-size:25px;margin-bottom:16px}.aside h3{font:700 13px Manrope;color:#344a9d;margin:0 0 8px}.aside p{font-size:10px;line-height:1.7;color:#7888c0;margin:0}.aside hr{border:0;border-top:1px solid #dce4ff;margin:18px 0}.aside p strong{color:#5368ad}.login-area{min-height:calc(100vh - 190px);display:grid;place-items:center}.login-card{width:min(390px,100%);background:white;border:1px solid #edf0f5;border-radius:14px;padding:32px 35px;text-align:center;box-shadow:0 8px 30px #273b6410}.login-card .brand-mark{margin:0 auto 18px}.login-card h1{font-size:24px}.login-card>p{font-size:11px;color:#929cad}.login-card form{text-align:left}.login-card form>.primary{width:100%}.login-note{display:block;color:#a3adba;margin-top:17px;font-size:9px!important}.notice{background:#eef8f2;border:1px solid #d9f0e2;color:#267b54;border-radius:9px;padding:11px 13px;display:flex;align-items:center;gap:10px;margin-bottom:20px;font-size:10px}.notice p{margin:0;flex:1;line-height:1.5}.notice button{border:0;background:none;color:#579777;font-size:17px}footer{padding:16px 42px;border-top:1px solid #e9edf3;display:flex;justify-content:space-between;color:#a6afbc;font-size:9px}footer span{margin-left:auto}.sidebar button:focus-visible,.primary:focus-visible,.secondary:focus-visible,.filters button:focus-visible{outline:3px solid #a9bbff;outline-offset:2px}@media(max-width:950px){.sidebar{width:215px}.main-content{margin-left:215px}.page{padding:32px 25px}.topbar{padding:0 25px}.form-layout{grid-template-columns:1fr 210px}}@media(max-width:720px){.sidebar{position:static;width:auto;padding:13px 16px;border:0;border-bottom:1px solid #e9edf3}.nav-caption,.sidebar-bottom{display:none}.sidebar nav{display:flex;margin-top:13px}.sidebar nav button{width:auto;height:35px;padding:0 10px;font-size:10px}.sidebar nav button b{display:none}.main-content{margin:0}.topbar{height:52px;padding:0 17px}.mode{font-size:0}.page{padding:26px 16px}.heading{align-items:flex-start;flex-direction:column}.heading>.primary{width:100%}.stats{gap:8px}.stats article{padding:12px 10px;min-height:92px}.stats article span{font-size:9px}.stats article strong{font-size:20px}.panel-head{align-items:flex-start;flex-direction:column;padding:16px 14px 12px}.search{width:100%}.filters{overflow-x:auto;padding:0 14px 12px}.filters button{white-space:nowrap}th,td{padding-left:10px;padding-right:10px}.form-layout{grid-template-columns:1fr}.aside{display:none}.form-panel{padding:18px 15px}footer{padding:14px 16px;font-size:8px;gap:10px}footer span{text-align:right}.login-card{padding:27px 21px}}@media(max-width:420px){.stats{grid-template-columns:1fr}.stats article{min-height:80px}.stats article strong{margin-top:3px}.crumb{font-size:9px}.login-area{min-height:60vh}}
</style>