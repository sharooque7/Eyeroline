import { Elements } from "@stripe/react-stripe-js";
import React, { useEffect, useState } from "react";
import CheckoutPage from "./CheckoutPage";
import { loadStripe } from "@stripe/stripe-js";
import { useAppDispatch } from "../../app/store/configureStore";
import agent from "../../app/api/agent";
import { setBasket } from "../basket/basketSlice";
import LoadingComponent from "../../app/layout/LoadingComponent";

const stripePromise = loadStripe(
  "pk_test_51MtWvBSDLKUJanGCFn15Pko8pMu9DAKKbWoqNCXFC6zTVIdV3zQu29V34jzJpzbuNcbbwI3evUj8lM9sDlTUNVuh00QD7bUXNA"
);
const CheckoutWrapper = () => {
  const dispatch = useAppDispatch();
  const [loading, setLoading] = useState(true);
  useEffect(() => {
    agent.Payments.createPaymentIntent()
      .then((basket) => dispatch(setBasket(basket)))
      .catch((error) => console.log(error))
      .finally(() => setLoading(false));
  }, [dispatch]);

  if (loading) return <LoadingComponent message="Loading checkout..." />;
  return (
    <Elements stripe={stripePromise}>
      <CheckoutPage />
    </Elements>
  );
};

export default CheckoutWrapper;
