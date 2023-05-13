import React, { useEffect } from 'react'
import { useStore } from '../providers/GlobalProvider'
import Widget from '../components/charts/widgets/Widget'
// import Featured from "../components/featured/Featured";
import Chart from '../components/charts/chart/Chart'

function Home (props) {
  useEffect(() => {
    // Para establecer en el módulo en el que nos encontramos
    props.setTitle("Dashboard");
  }, []);
  /* isOpen (globalstate) -> para que el contenido se ajuste según el ancho de la sidebar (navegación) */
  const isOpen = useStore((state) => state.sidebar)
  
  return (
    <div className={isOpen === true ? "wrapper" : "side"}>
      <div className="home">
        <div className="homeContainer">
          <div className="widgets">
            <Widget type={"ventasMes"} />
            <Widget type={"bolsasVend"} />
            <Widget type={"cliente"} />
            <Widget type={"bolsasDisp"} />
          </div>
          <div className="charts">
            {/* <Featured /> */}
            <Chart />
          </div>
        </div>
      </div>
    </div>
  )
}

export default Home