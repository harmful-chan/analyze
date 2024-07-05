import { createRouter, createWebHistory } from "vue-router"

const routes = [
    {
        path: "/",  // http://localhost:5173/
        component: ()=> import("@/views/index.vue"),
        children:[
            {
                path: "address", // http://localhost:5173/address
                component: ()=> import("@/views/address.vue")
            },
            {
                path: "mark", // http://localhost:5173/mark
                component: ()=> import("@/views/mark.vue")
            },
        ]
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

export default router