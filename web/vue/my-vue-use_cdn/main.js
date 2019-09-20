// 非同期コンポーネント
Vue.component('async-example', function (resolve, reject) {
    setTimeout(function () {
      // resolve()コールバックにコンポーネント定義を渡します
      resolve({
        //コンポーネント定義
        template: '<div>I am async!</div>'
      })
    }, 1000)
});


/* 
    Vur ルート
 */
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
        キーボード入力値: "k-e-y",
    },

    //監視するぞい
    watch:{
        toggle: function(newvalue, oldvalue) {
            console.log(newvalue.toString() + " " + oldvalue.toString())
        }
    },

    mounted() {

        document.onkeydown = e => {
    
            this.キーボード入力値 = e.key.toString();

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
            this.キーボード入力値="do"
        }
    }
})