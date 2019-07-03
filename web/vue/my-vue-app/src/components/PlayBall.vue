<template>
    
</template>

<script lang="ts">
import Vue from 'vue'
export default Vue.extend({
    
})

// Vue + TypeScript。https://jp.vuejs.org/v2/guide/typescript.html
const TypeInferenceComponent_型推論版 = Vue.extend({
    //  TypeScript の型推論を有効にする。＝＞　Vue.extend({〜〜})
})
  
const Component_非型 = {
    // これは型推論を持っていません、
    // なぜなら、これは Vue コンポーネントのオプションであるということを伝えることができないためです。
}
</script>
