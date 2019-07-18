// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import store from './store.js'

Vue.config.productionTip = false

/* eslint-disable no-new */
new Vue({
  el: '#app',

  /*
    vuexテスト　ここでルート登録により、以下全てのコンポーネントから参照可能になる。
    「$store」として参照する。
  */
  store, 
  
  components: { App },
  template: '<App/>'
})
