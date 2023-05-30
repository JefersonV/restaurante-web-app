import { Button, Table } from "reactstrap";
import { HiPencilAlt } from "react-icons/hi";
import { RiDeleteBin6Line } from "react-icons/ri";
import SwalDelete from "./ModalDeletePurchase"
import ModalEdit from "./ModalEditPurchase"
import dayjs from "dayjs";

function TablePurchases(props) {
  const { data, actualizarListaCompras } = props;

  return (
    <Table hover responsive>
      <thead>
        <tr>
          <th>#</th>
          <th style={{ textAlign: "left" }}>No. Documento</th>
          <th style={{ textAlign: "left" }}>Fecha</th>
          <th style={{ textAlign: "left" }}>Concepto</th>
          <th style={{ textAlign: "left" }}>Proveedor</th>
          <th style={{ textAlign: "left" }}>Total</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        {data.length === 0 ? (
          <tr>
            {/* Si el filtrado de la búsqueda es = [] */}
            <td colSpan={7}>
              Resultados no encontrados, el registro no existe ..
            </td>
          </tr>
        ) : (
          /* Si filtró con éxito */
          data.map((compra, index) => {
            return (
              <tr key={index}>
                <th scope="row">{index + 1}</th>
                <td style={{ textAlign: "left" }}>{compra.numeroDocumento}</td>
                <td style={{ textAlign: "left" }}>{dayjs(compra.fecha).format('DD/MM/YYYY')}</td>
                <td style={{ textAlign: "left" }}>{compra.concepto}</td>
                <td style={{ textAlign: "left" }}>{compra.proveedor}</td>
                <td style={{ textAlign: "left" }}>{`Q.${compra.total.toFixed(
                  2
                )}`}</td>
                <td>
                  <button>
                    <ModalEdit
                      idPlatillo={compra.idGasto}
                      actualizarListaMenu={actualizarListaCompras}
                    />
                  </button>
                  <SwalDelete
                    idplatillo={compra.idGasto}
                    actualizarListaMenu={actualizarListaCompras}
                  />
                </td>
              </tr>
            );
          })
        )}
      </tbody>
    </Table>
  );
}

export default TablePurchases;
