<template>
  <div id="app">
    <img src="./assets/logo.png">
    <HelloWorld/>
  <div id="dapp">
    <button v-on:click="onClick">Clickable</button>
  </div>
  </div>
</template>

<script>
import HelloWorld from './components/HelloWorld'

export default {
  name: 'app',
  components: {
    HelloWorld
  },
  methods: {
    onClick: function(event){
      console.error(event.target)

      console.log( 0 )
      console.log( this.$store.state.count )
    },

  }
  
}
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 0px;
}
</style>
