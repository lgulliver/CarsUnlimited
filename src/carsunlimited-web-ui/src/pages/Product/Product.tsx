import { Grid, Skeleton, Typography } from "@mui/material";
import React from "react";
import { useParams } from "react-router-dom";
import Layout from "../../layouts/MainLayout";

import styles from "./Product.module.css";
//import {  blue, deepPurple, green, grey, deepOrange } from "@mui/material/colors";
//import Rating from "../../components/Rating/Rating";
import ProductPageTabs from "../../components/ProductPageTabs/ProductPageTabs";
import { InventoryItem, InventoryImage } from "../../models/InventoryItem.d";
import axios from "axios";
import { useQuery } from "react-query";
import AddToCart from "../../components/AddToCart/AddToCart";

type ProductProps = {
  manufacturer: string;
  model: string;
};

const API_URL = process.env.REACT_APP_INVENTORY_API_URL;

const fetchProduct = async (manufacturer: string, model: string) => {
  const response = await axios.get(
    `${API_URL}/inventory/${manufacturer}/${model}`
  );

  return response.data;
};

const getInventoryImage = (images: InventoryImage[]) => {
  return images.find((image: InventoryImage) => image.isPrimary) || images[0] || "https://dummyimage.com/300x200/eee/aaa.png&text=No+image+available";
};

function ProductInfo({ manufacturer, model }: ProductProps) {
  const { isLoading, error, data } = useQuery(`${manufacturer}-${model}`, () => fetchProduct(manufacturer, model));

  if (isLoading) {
    return (
      <div>
        <Grid container spacing={{ xs: 2, md: 3 }} columns={{ xs: 4, sm: 8, md: 12 }}>
          <Grid item xs>
            <Skeleton variant="rectangular" width={345} height={250} />
          </Grid>
          <Grid item xs>
            <Skeleton variant="rectangular" width={345} height={250} />
          </Grid>
          <Grid item xs>
            <Skeleton variant="rectangular" width={345} height={250} />
          </Grid>
        </Grid>
      </div>
    );
  } 
  if (error) {
    return (
      <Grid item xs>
        Error!
      </Grid>
    );
  }

  const product: InventoryItem = data;
  const inStock = product.inStock > 0 ? true : false;

  return (      
        <Grid container spacing={0}>
          <Grid item xs={7}>
            <img src={getInventoryImage(product.images).imageUrl} width={600} height={350} alt="{product.manufacturer} {product.model}" />
          </Grid>
          <Grid item xs={5}>
            <Grid container spacing={2}>
              <Grid item xs={12}>
                <h1>
                  {product.manufacturer} {product.model}
                </h1>
              </Grid>
              <Grid item xs={12}>
                <strong>Manufacturer:</strong> {product.manufacturer}
                {/* <Rating /> */}
              </Grid>
              {/* <Grid item xs={12}>
                Available colours: <br />
                <Stack direction="row" spacing={1}>
                  <Avatar sx={{ bgcolor: grey[500]}}>&nbsp;</Avatar>
                  <Avatar sx={{ bgcolor: deepPurple[500]}}>&nbsp;</Avatar>
                  <Avatar sx={{ bgcolor: green[800]}}>&nbsp;</Avatar>
                  <Avatar sx={{ bgcolor: blue[800]}}>&nbsp;</Avatar>
                  <Avatar sx={{ bgcolor: deepOrange[800]}}>&nbsp;</Avatar>
                </Stack>
              </Grid> */}
              <Grid item xs={12}>
                <br />
                <Typography variant="h3" gutterBottom component="div">
                  &pound;{product.price.toLocaleString(navigator.language, { minimumFractionDigits: 2 })}
                </Typography>
              </Grid>
              <Grid item xs={12}>
                {
                  /*
                  THIS IS A GAPING SECURITY ISSUE. DON'T DO THIS IRL! OPEN TO SUGGESTIONS ON FIXING IT
                  */
                }
                <AddToCart id={product.id} price={product.price} inStock={inStock} />
              </Grid>
            </Grid>
          </Grid>
          <Grid item xs={12}>
            <ProductPageTabs product={product} />
          </Grid>
        </Grid>
  );
}

function Product() {
  let { manufacturer, model } = useParams();
  let manufacturerName = manufacturer || '';
  let modelName = model || '';

  return (
    <Layout>
      <br />
      <div className={styles.Product} data-testid="Product">
        <ProductInfo manufacturer={manufacturerName} model={modelName} />
      </div>
    </Layout>
  );
};

export default Product;
