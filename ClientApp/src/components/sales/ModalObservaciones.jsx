import React, { useState } from 'react';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter, FormGroup, Label, Col, Input } from 'reactstrap';
import { AiOutlineEye } from "react-icons/ai"
function ModalObservaciones(props) {
  const [modal, setModal] = useState(false);

  const toggle = () => setModal(!modal);

  return (
    <div>
      <Button color="danger" onClick={toggle}>
      <AiOutlineEye/> Observaciones
      </Button>
      <Modal isOpen={modal} toggle={toggle} centered>
        <ModalHeader toggle={toggle}><AiOutlineEye size={28}/> Observaciones</ModalHeader>
        <ModalBody>
        <FormGroup>
            <Label
            for="exampleText"
            sm={2}
            >
            Comentario
            </Label>
            <Col sm={12}>
            <Input
                id="exampleText"
                name="text"
                type="textarea"
                onChange={props.changeObservaciones}
            />
            </Col>
        </FormGroup>
        </ModalBody>
        <ModalFooter>
          <Button color="primary" onClick={toggle}>
            Aceptar
          </Button>
          <Button color="secondary" onClick={toggle}>
            Cancel
          </Button>
        </ModalFooter>
      </Modal>
    </div>
  );
}

export default ModalObservaciones;