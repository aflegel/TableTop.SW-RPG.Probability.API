import * as React from 'react';
import { Route } from 'react-router-dom';
import { Layout } from './components/Layout';
import Home from './components/Home';
import DiceStatistics from './components/DiceStatistics';

export const routes = <Layout>
    <Route exact path='/' component={ Home } />
	<Route path='/diceStatistics/' component={ DiceStatistics } />
</Layout>;
