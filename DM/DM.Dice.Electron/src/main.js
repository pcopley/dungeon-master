import Vue from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify';
import Vue2Filters from 'vue2-filters'

Vue.use(Vue2Filters)

Vue.config.productionTip = false

new Vue({
  vuetify,
  render: h => h(App)
}).$mount('#app')
