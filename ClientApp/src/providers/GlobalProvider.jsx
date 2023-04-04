import { create } from 'zustand'

/* Estado global para la sidebar */
export const useStore = create((set) => ({
  sidebar: true,
  showSidebar: () => set((state) => ({ sidebar: !state.sidebar })),
}))

/* const [subNav, setSubNav] = useState(false)
  const showSubNav = () => setSubNav(!subNav) */
export const useSubItem = create((set) => ({
  subNav: false,
  showSubNav: () => set((state) => ({ subNav: !state.subNav })),
}))

export const useTitle = create((set) => ({
  title: '',
  setTitle: () => set((state) => ({title: state.title}))
}))