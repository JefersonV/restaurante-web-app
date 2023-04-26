
import React, { useState } from 'react' 
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap'
import { FcPlus } from 'react-icons/fc'
import { AiOutlineArrowRight } from 'react-icons/ai'
import '../../styles/SaleModal.scss'
import { Link, useResolvedPath } from 'react-router-dom'
function ModalNewSale() {
  const [modal, setModal] = useState(false)
  
  const toggle = () => setModal(!modal)

  const url = useResolvedPath("").pathname;
  return (
    <>
      <Button 
        color="danger"
        outline
        onClick={toggle}
      >
        <FcPlus />
        Nueva venta
      </Button>
      <Modal isOpen={modal} fade={false} toggle={toggle} centered={true}>
        <ModalHeader toggle={toggle}>Seleccione el tipo de venta</ModalHeader>
        <ModalBody>
          <div className="container-fluid">
            <div className="row">
              <div className="col">
                <div className="sale-container sale-container--individual">
                  <label className="sale-label" htmlFor="venta-individual">Venta Individual</label>
                  <Link
                    to="/new"
                    className="btn-sale btn-sale--individual"
                    id="venta-individual"
                  >
                    <AiOutlineArrowRight
                      size={26}
                      color="#FFFFFF"
                      />
                  </Link>
                </div>
              </div>
              <div className="col">
                <div className="sale-container sale-container--grupal">
                  <label className="sale-label" htmlFor="venta-individual">Venta Grupal</label>
                  <Link
                    to="/group"
                    className="btn-sale btn-sale--individual"
                    id="venta-individual"
                    >
                    <AiOutlineArrowRight
                      size={26}
                      color="#FFFFFF"
                    />
                  </Link>
                </div>
              </div>
            </div>
          </div>
        </ModalBody>
      </Modal>
    </>
  )
}

export default ModalNewSale