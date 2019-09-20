// .vueファイルはトランスパイルの必要があるから、これだけでは使えない。　WebpackとかBrowserify・・・。

<template lang="pug">
//- 1行コメント
//
    （複数行コメント）
    # pugの書き方
    タグ
        閉じタグはなく、インデントで明示する。
    タグの属性　
        `(〜)`丸括弧に記述
            例)
                a(href="example.com", target="_blank") aタグです
        ID属性は　`#`　でも可能
        クラス属性は　`.`　でも可能
    
div #drag-and-drop-area (@drop="onDrop")
    ドラッグエリア    

</template>

<script>
export default {
    created:{

    },

    mounted:{

    },

    methods:{
        onDrop(event){

        },
        onDrag(){
            console.log("onDrag");
        },
        onDrop(){
            console.log("onDrop");
        }
    }
}
</script>

<style>

</style>
