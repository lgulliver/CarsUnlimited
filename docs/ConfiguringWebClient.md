# Configuring the Web Client

The web client is built using React with Typescript.

It uses `dotenv` to load environment variables from a `.env` file.

There are some basic feature toggles to hide functionality that is not yet finished.

It also contains the location of the APIs.

```env
REACT_APP_INVENTORY_API_URL=http://localhost:5010/api
REACT_APP_CART_API_URL=http://localhost:5020/api
REACT_APP_PURCHASE_API_URL=http://localhost:5030/api
REACT_APP_FEATURE_FLAG_SEARCH=0
REACT_APP_FEATURE_FLAG_OFFERS=0
```

For hosted environments, the API URLs will need to be set to the appropriate values.

These can also be overridden with environment variables that share the same name.