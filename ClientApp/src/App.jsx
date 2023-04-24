import { useState } from 'react'
import './styles/App.scss'
import Home from './pages/Home'
import Inventory from './pages/Inventory'
import Purchases from './pages/Purchases'
import Sales from './pages/Sales'
import CashBox from './pages/CashBox'
import Reports from './pages/Reports'
import Users from './pages/Users'
import Config from './pages/Config'
import Sidebar from './components/sidebar/Sidebar'
import Menu from './pages/Menu'
import Providers from './pages/Providers'
import Customers from './pages/Customers'
import {
  BrowserRouter,
  Routes,
  Route,
  Link,
} from "react-router-dom";
import { FaCashRegister } from "react-icons/fa"
import { SkeletonTheme } from 'react-loading-skeleton'

function App() {
  const [pageTitle, setPageTitle] = useState("")

  const setTitle = (title) => {
    setPageTitle(title)
  }

  return (
    <>
      <SkeletonTheme baseColor='#313131' highlightColor='#525252'>
        <BrowserRouter>
          {/* Sidebar */}
          <Sidebar pageTitle={pageTitle}>
            <Routes>
              <Route
                exact path="/" 
                element={<Home setTitle={setTitle} /> }
                />
              <Route
                path="/inventory"
                element={<Inventory setTitle={setTitle} />}
              />
              <Route
                path="/sales"
                element={<Sales setTitle={setTitle} />}
              />
              <Route
                path="/purchases"
                element={<Purchases setTitle={setTitle} />}
              />
              <Route
                path="/cash-box"
                element={<CashBox setTitle={setTitle} />}
              />
              <Route
                path="/users"
                element={<Users setTitle={setTitle} />}
              />
              <Route
                path="/reports"
                element={<Reports setTitle={setTitle} />}
              />
              <Route
                path="/config"
                element={<Config setTitle={setTitle} />}
              />
              <Route
                path="/menu"
                element={<Menu setTitle={setTitle} />}
              />
              <Route
                path="/providers"
                element={<Providers setTitle={setTitle} />}
              />
              <Route
                path="/customers"
                element={<Customers setTitle={setTitle} />}
              />
            </Routes>
          </Sidebar>
        </BrowserRouter>

      </SkeletonTheme>
    </>
  )
}

export default App
