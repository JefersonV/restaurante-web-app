import { create } from 'zustand'

/* Estado global para el buscador */
export const useSearch = create((set) => {
  search: ""
})
/* Estado global para la sidebar */
export const useStore = create((set) => ({
  sidebar: true,
  showSidebar: () => set((state) => ({ sidebar: !state.sidebar })),
}))
