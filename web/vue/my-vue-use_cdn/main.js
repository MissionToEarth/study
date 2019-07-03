var first_root_app = new Vue ({
    // id=appの部分に対応する。
    el: "#app",

    // 保持するデータの宣言
    data: {
        menu_title: 'sayssaysay',
        list: ["wand",'dondon'],
        toggle: true,
        message: "init",
        TAパラ切り替えスイッチ: true,   //ソースコードにUTF-8が使えるんだぜ。使うぜ。先頭文字にascii文字を付けるのは命名規則としてありかもしれない。
    },

    // 関数定義
    methods: {
        fサラダ: function() {
            console.log(first_root_app.menu_title)
        },
        onButton: function(event){
            console.log("button down")
            this.toggle = !this.toggle
        }
    }
})