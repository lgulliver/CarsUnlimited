import { Grid, Typography } from "@mui/material";
import React from "react";
import axios from "axios";
import { useQuery } from "react-query";
import { InventoryItem } from "../../models/InventoryItem.d";
import ProductCard from "../ProductCard/ProductCard";
import Skeleton from '@mui/material/Skeleton';
//import styles from "./LatestProducts.module.css";

type LatestProductsProps = {
  category: string;
  displayName?: string;
};

const API_URL = process.env.REACT_APP_INVENTORY_API_URL;

const fetchLatestProducts = async (category: string) => {
  const response = await axios.get(
    `${API_URL}/inventory/${category}/latest`
  );
  return response.data;
};

const LatestProducts = ({category}: LatestProductsProps) => {

  const { isLoading, error, data } = useQuery(`latest-${category}-products`, () => fetchLatestProducts(category));
  if (isLoading) {
    return (
      <Grid container spacing={3}>
        <Grid item xs>
          <Skeleton variant="rectangular" width={345} height={400} />
        </Grid>
        <Grid item xs>
          <Skeleton variant="rectangular" width={345} height={400} />
        </Grid>
        <Grid item xs>
          <Skeleton variant="rectangular" width={345} height={400} />
        </Grid>
      </Grid>
    );
  } 
  if (error) {
    return (
      <Grid item xs>
        Error!
      </Grid>
    );
  }
  if(data.length === 0) {
    return (
      <Grid item xs>
        No products to display
      </Grid>
    );
  }

  return data?.map((product: InventoryItem) => (
    <Grid item xs>
      <ProductCard item={product} />
    </Grid>
  ));
  
}

const LatestProductsComponent = ({category, displayName}: LatestProductsProps) => {
  return (
    <Grid container spacing={3}>
      <Grid item xs={12}>
        <Typography
          variant="h6"
          color="inherit"
          noWrap
          sx={{ borderBottom: 1, borderColor: "divider" }}
          style={{ textTransform: "uppercase" }}
        >
          <strong>LATEST {displayName}</strong>
        </Typography>
      </Grid>
      <LatestProducts category={category} />
    </Grid>
  );
};

export default LatestProductsComponent;
