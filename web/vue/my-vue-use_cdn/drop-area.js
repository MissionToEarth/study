//　ファイルとかをドロップするエリア。
Vue.component('drag-drop-area', {
    name: "ドラドラ",

    data: function () {
        return {
            name: "dd"
        }
    },
    methods:{
        onMouseLeave: function(){
            console.log("離れる");
        },
        onMouseOver: function(){
            console.log("マウスオーバー");
        },
        onDrop: function()
        {
            console.log("ドロップ");
        }
    },
    template: '<div id="dd-area" @mouseover="onMouseOver" @mouseleave="onMouseLeave">ああああ</div>' 
})
