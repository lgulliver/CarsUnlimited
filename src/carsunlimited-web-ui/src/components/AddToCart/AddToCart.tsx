import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Button } from '@mui/material';
import axios from 'axios';
import toast, { Toaster } from 'react-hot-toast';
import { useMutation, useQueryClient } from 'react-query';
import React from 'react';

type AddToCartProps = {
  id: string;
  inStock: boolean;
  price: number;
};

const API_URL = process.env.REACT_APP_CART_API_URL;

const addItemToCart = async ({ id, price }: AddToCartProps) => {
  
  const sessionId = localStorage.getItem("sessionId") || '';
  const headers = {
    'X-CarsUnlimited-SessionId': sessionId
  }

  const cartItem = { id: id, price: price, count: 1 };
  
  await axios.post(`${API_URL}/cart/add-to-cart`, cartItem, { headers })                             
              .catch(error => {
                console.error('An error occurred!', error);
              });
}

export default function AddToCart({ id, price, inStock = true }: AddToCartProps) {
  const client = useQueryClient();
  const mutation = useMutation(addItemToCart, {
    onSuccess: () => {
      client.invalidateQueries('cart-item-count');
      toast.success('Item added to cart!');
    },
  });

  if(inStock) {
    return (
      <div>
        <Button
          variant="contained"
          color="primary"
          size="large"
          startIcon={<FontAwesomeIcon icon="cart-plus" />}
          onClick={() => {mutation.mutate({ id, price, inStock })}}
        >
          Add to cart
        </Button>
        <Toaster />
      </div>
    );
  } else {
    return (
      <div>
        <Button
          variant="contained"
          color="error"
          size="large"
          startIcon={<FontAwesomeIcon icon="cart-plus" />}
          disabled
        >
          Out of stock
        </Button>
        <Toaster />
      </div>
    );
  }
}