import '../../helpers/FontAwesome';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Button } from '@mui/material';
import React from 'react';
import axios from 'axios';
import { useQuery } from 'react-query';

const API_URL = process.env.REACT_APP_CART_API_URL;
const sessionId = localStorage.getItem("sessionId") || '';
const headers = {
  'X-CarsUnlimited-SessionId': sessionId
}

const getCardItemCount = async () => {
  const response = await axios.get(`${API_URL}/cart/get-cart-items-count`, { headers })
  return response.data
}

export default function CartButton() {
  // eslint-disable-next-line
  const { isLoading, error, data } = useQuery('cart-item-count', getCardItemCount);

  return (
    <div>
      <Button variant="outlined" href="/Cart" startIcon={<FontAwesomeIcon icon={['fas', 'shopping-basket']} />}>
        {data || 0}
      </Button>
    </div>
  )
}
