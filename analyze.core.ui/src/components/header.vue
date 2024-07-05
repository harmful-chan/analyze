<script setup>
import {useRouter} from 'vue-router'

const router  = useRouter()

const jump_address = ()=>{
    router.push('/address')
}

const jump_mark = ()=>{
    router.push('/mark')
}


</script>

<template>
    <el-menu  class="el-menu-demo" mode="horizontal"  >
    <el-sub-menu index="1">
      <template #title>订单操作</template>
      <el-menu-item index="1-1" @click="jump_address">获取订单地址</el-menu-item>
      <el-menu-item index="1-2" @click="jump_mark">订单声明发货</el-menu-item>
    </el-sub-menu>
  </el-menu>
</template>

<style scoped> 

</style>
