import 'babel-polyfill'
import Vue from 'vue'
import Vuex from 'vuex'
//プラグインとして登録
Vue.use(Vuex)

//      シンプルなストア構造
//ストアを作成
const store = new Vuex.Store({
    state: {
        count: 0,
        msg: 'store_count'
    },
    mutations: {
        // カウントアップするミューテーションを登録
        increment(state) {
            state.count++
        }
    }
})

export default store
/*
//使用方法
import store from 'src/xxx/store.js'
console.log(store.state.count)
// ミューテーションの呼び出し方法　commit を使う
store.commit('increment')
個別にインポートするのが面倒な時は、アプリケーション・ルートに登録する。
ここでは、main.jsで　importして登録してやる。
 */