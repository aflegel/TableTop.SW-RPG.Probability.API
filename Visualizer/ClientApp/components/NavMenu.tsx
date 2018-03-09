import * as React from 'react';
import { NavLink, Link } from 'react-router-dom';

export class NavMenu extends React.Component<{}, {}> {
	public render() {
		return <div className="navbar-fixed">
			<nav className="white">
				<div className="nav-wrapper">
					<div className="container">
						<div className="logo-container">
							<Link className='navbar-brand' to={'/'}>Visualizer</Link>
						</div>
					</div>
				</div>
			</nav>
		</div>;
	}
}
