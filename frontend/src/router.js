import { createRouter, createWebHistory } from 'vue-router'
import TicketList from './pages/TicketList.vue'
import TicketNew from './pages/TicketNew.vue'
import Login from './pages/Login.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', name: 'activities', component: TicketList, meta: { title: 'Мои активности' } },
    { path: '/new', name: 'activity-new', component: TicketNew, meta: { title: 'Новая запись' } },
    { path: '/login', name: 'login', component: Login, meta: { title: 'Вход в систему' } },
  ],
  scrollBehavior: () => ({ top: 0 }),
})
export default router
