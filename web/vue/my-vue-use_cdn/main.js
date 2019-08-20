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
        key_input: "k-e-y",
    },

    //監視するぞい
    watch:{
        toggle: function(newvalue, oldvalue) {
            console.log(newvalue.toString() + " " + oldvalue.toString())
        }
    },

    mounted() {

        document.onkeydown = e => {
    
            this.key_input = e.key.toString();

          // cmd + s は 動く
          if (e.key === 'S' && e.shiftKey) {
            
            alert("キーダウン")
            
            return false
          }
        }
    },    

    // 関数定義
    methods: {
        fサラダ: function() {
            console.log(first_root_app.menu_title)
        },
        onButton: function(event){
            console.log("button down")
            this.toggle = !this.toggle
        },
        onPageDown: function(event){
            console.error(JSON.stringify(event,null,"\t"))
            alert("")
        },
        doSomething: function(event){
            alert("do something")
            this.key_input="do"
        }
    }
})