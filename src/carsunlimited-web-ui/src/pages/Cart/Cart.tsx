import { Button, Divider, Grid } from '@mui/material';
import axios from 'axios';
import React from 'react';
import { useMutation, useQuery, useQueryClient } from 'react-query';
import Layout from '../../layouts/MainLayout';
import styles from './Cart.module.css';

import { CartItem } from '../../models/CartItem.d';
import CartItemCard from '../../components/CartItemCard/CartItemCard';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import toast from 'react-hot-toast';


const CART_API_URL = process.env.REACT_APP_CART_API_URL;
const PURCHASE_API_URL = process.env.REACT_APP_PURCHASE_API_URL;

const sessionId = localStorage.getItem("sessionId") || '';
const headers = {
  'X-CarsUnlimited-SessionId': sessionId
}

const fetchCartItems = async () => {
  const response = await axios.get(`${CART_API_URL}/cart/get-cart-items`, { headers })
  return response.data
}

const CartTotal = () => {
  const { isLoading, error, data } = useQuery('cart-items', fetchCartItems);

  

  if (isLoading) {
    return <div>Loading...</div>
  }

  if (error) {
    return <div>Error!</div>
  }

  if(data.items.length === 0) {
    return <div>£0.00</div>;
  }

  return <div>£{data.total.toLocaleString(navigator.language, { minimumFractionDigits: 2 })}</div>;
}


const CartItems = () => {
  const { isLoading, error, data } = useQuery('cart-items', fetchCartItems);

  if (isLoading) {
    return <div>Loading...</div>
  }

  if (error) {
    return <div>Error!</div>
  }

  if(data.items.length === 0) {
    return (
      <Grid item xs>
        No items in cart.
      </Grid>
    );
  }

  return data.items?.map((item: CartItem) => (
    <Grid item xs={12}>
      <CartItemCard item={item} />
      <Divider />
    </Grid>
  ));
};

const clearCart = async () => {
  const sessionId = localStorage.getItem("sessionId") || '';
  const headers = {
    'X-CarsUnlimited-SessionId': sessionId
  }

  await axios.get(`${CART_API_URL}/cart/delete-cart`, { headers })
              .catch(error => {
                console.error('An error occurred!', error);
              });
};

const completeCart = async () => {
  const sessionId = localStorage.getItem("sessionId") || '';

  await axios.post(`${PURCHASE_API_URL}/purchase/${sessionId}`)
              .catch(error => {
                console.error('An error occurred!', error);
              });
};

export default function Cart() {
  //eslint-disable-next-line
  const { isLoading, error, data } = useQuery('cart-item-count');
  const client = useQueryClient();
  const clearCartMutation = useMutation(clearCart, {
    onSuccess: () => {
      client.invalidateQueries('cart-items');
      client.invalidateQueries('cart-item-count');
      toast.success('Cart cleared.');
    }
  });

  const completeCartMutation = useMutation(completeCart, {
    onSuccess: () => {
      client.invalidateQueries('cart-items');
      client.invalidateQueries('cart-item-count');
      toast.success('Cart completed.');
    }
  });

  return (
    <Layout>
      <div className={styles.Cart} data-testid="Cart">
        <h1>Cart ({data})</h1>
        <Grid container spacing={{ xs: 1 }}>
          <Grid item xs={2}></Grid>
          <Grid item xs={2}>
            <strong>Product</strong>
          </Grid>
          <Grid item xs={2}>
            <strong>Unit Price</strong>
          </Grid>
          <Grid item xs={2}>
            <strong>Quantity</strong>
          </Grid>
          <Grid item xs={2}>
            <strong>Total Price</strong>
          </Grid>
          <Grid item xs={2}></Grid>        
        </Grid>
        <Divider sx={{marginBottom: '5px'}} />
        <Grid container spacing={{ xs: 1 }}>    
          <CartItems />
        </Grid>
        <Divider />
        <Grid container spacing={{ xs: 1 }}>    
          <Grid item xs={8} sx={{textAlign: 'right'}}>
            <strong>Total:</strong>
          </Grid>
          <Grid item xs={4}>
            <CartTotal />
          </Grid>
        </Grid>
        <Divider sx={{marginBottom: '10px'}}/>
        <Grid container spacing={{ xs: 1 }}>    
          <Grid item xs={7}></Grid>
          <Grid item xs={5} sx={{textAlign: 'right'}}>
            <Button
              variant="contained"
              color="primary"
              startIcon={<FontAwesomeIcon icon="shopping-cart" />}
              onClick={() => completeCartMutation.mutate()}
            >
              Checkout
            </Button>
            <Button
              variant="contained"
              color="error"
              startIcon={<FontAwesomeIcon icon="trash-alt" />}
              onClick={() => clearCartMutation.mutate()}
            >
              Clear Cart
            </Button>
          </Grid>
        </Grid>
      </div>
    </Layout>
  );
};
