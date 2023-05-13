import { create } from "zustand";

const useAuthStore = create((set) => ({
    isAuth: false,
    token: "",
    nombreUsuario: "",
    rol: "",
    setUser: (nesAuth, newToken, newNombreUsuario, newRol) =>
        set(() => ({
            isAuth: nesAuth,
            token: newToken,
            nombreUsuario: newNombreUsuario,
            rol: newRol,
        })),
}));

export default useAuthStore;
