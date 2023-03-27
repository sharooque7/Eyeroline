import { Button, ButtonGroup, Typography } from "@mui/material";
import React from "react";
// import { useDispatch, useSelector } from "react-redux";
import { useAppDispatch, useAppSelector } from "../../app/store/configureStore";
// import { decrement, increment } from "./counterReducer";
import { decrement, increment } from "./counterSlice";

const ContactPage = () => {
  const dispatch = useAppDispatch();
  const { data, title } = useAppSelector((state) => state.counter);
  return (
    <>
      <Typography variant="h2">{title}</Typography>
      <Typography variant="h5">{data}</Typography>
      <ButtonGroup>
        <Button onClick={() => dispatch(decrement(1))}>Increment</Button>
        <Button onClick={() => dispatch(increment(1))}>Decrement</Button>
        <Button onClick={() => dispatch(increment(5))}>Decrement by 5</Button>
      </ButtonGroup>
    </>
  );
};

export default ContactPage;
